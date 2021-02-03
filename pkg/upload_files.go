package pkg

import (
	"bytes"
	"fmt"
	"github.com/bmatcuk/doublestar"
	"github.com/pkg/errors"
	"io"
	"io/ioutil"
	"mime/multipart"
	"net/http"
	"os"
	"path/filepath"
	"strings"
	"testhub-cli/pkg/console"
)

type UploadFilesParameters struct {
	FilePattern      string
	OrgAndProject    string
	Build            string
	ContextDir       string
	Branch           string
	IsCoverage       bool
	IsTestRun        bool
	ApiToken         string
	testhubApiDomain string
	isOnPremise      bool
}

const defaultTesthubDomain = "https://test-hub-api.azurewebsites.net"
const ApiKeyHeader = "ApiToken"

func (u *UploadFilesParameters) UploadTestResultFiles() error {
	u.loadConfig()
	if len(u.ApiToken) == 0 {
		t, err := getApiKey(u.OrgAndProject, u.testhubApiDomain, u.isOnPremise)
		if err != nil {
			fmt.Printf("Error retreiving API Token. Err: %s\n", err.Error())
			return fmt.Errorf("Api Token is missing")
		}
		u.ApiToken = strings.Replace(t, "\"", "", -1)
	}

	root := ""
	if len(u.ContextDir) != 0 {
		root = u.ContextDir + string(filepath.Separator)
	}

	pattern := root + u.FilePattern
	console.PrintLn("Using pattern %s", pattern)
	files, err := doublestar.Glob(pattern)
	if err != nil {
		return errors.Wrap(err, "Error listing the files")
	}

	console.PrintLn("%d files found", len(files))
	for _, f := range files {
		console.PrintLn("Uploading file %s", f)

		if u.IsTestRun {
			continue
		}

		err = u.uploadFile(f, u.IsCoverage)
		if err != nil {
			console.PrintError("Fail to upload file %s. Error: %v", f, err)
		}
	}

	err = u.printTestHubUrl(err)
	if err != nil {
		return err
	}

	return nil
}

func (u *UploadFilesParameters) printTestHubUrl(err error) error {
	org, proj, err := getOrgAndProject(u.OrgAndProject)
	if err != nil {
		return err
	}
	testhubUrl := u.getTestHubApiUrl(org, proj, u.Build)
	console.PrintGreen("Test results are available at: %s", testhubUrl)
	return nil
}

func (u *UploadFilesParameters) getTestHubApiUrl(org string, proj string, build string) string {
	domain := os.Getenv("TESTHUB_DOMAIN")
	return fmt.Sprintf("https://%s/api/%s/projects/%s/runs/%s", domain, org, proj, build)
}

func (u *UploadFilesParameters) loadConfig() {
	u.testhubApiDomain = os.Getenv("TESTHUB_DOMAIN")

	if len(u.testhubApiDomain) == 0 {
		u.testhubApiDomain = defaultTesthubDomain
	}

	if !strings.HasPrefix(u.testhubApiDomain, "https://") && !strings.HasPrefix(u.testhubApiDomain, "http://") {
		u.testhubApiDomain = "https://" + u.testhubApiDomain
	}

	p := os.Getenv("ON_PREMISE")
	u.isOnPremise = strings.EqualFold(p, "true") || strings.EqualFold(p, "1")

}

func (u *UploadFilesParameters) uploadFile(f string, isCoverage bool) error {

	org, proj, err := getOrgAndProject(u.OrgAndProject)
	if err != nil {
		return err
	}
	url := u.getTestHubApiUrl(org, proj, u.Build)
	fileParamName := "testResult"
	if isCoverage {
		file := mustOpen(f)
		err := u.uploadCoverage(org, proj, u.Build, file)
		return err
	} else {
		values := map[string]io.Reader{
			fileParamName: mustOpen(f),
			"branch":      strings.NewReader(u.Branch),
		}
		err := upload(url, values, u.ApiToken)
		return err
	}
}

func (u *UploadFilesParameters) uploadCoverage(org string, proj string, build string, file *os.File) error {
	url := u.getTestHubApiUrl(org, proj, build) + "/coverage"

	req, err := http.NewRequest(http.MethodPut, url, file)
	if err != nil {
		return errors.Wrap(err, "Error creating http request")
	}

	req.Header.Set(ApiKeyHeader, u.ApiToken)
	client := &http.Client{}
	res, err := client.Do(req)

	s, err := ioutil.ReadAll(res.Body)
	if err != nil {
		console.PrintError("Error reading response body. Error %v", err)
		return err
	}
	console.PrintLn("Response body %v", string(s))

	defer res.Body.Close()
	return nil
}

func upload(url string, values map[string]io.Reader, apiKey string) (err error) {
	console.PrintLn("Sending to url: %s", url)
	client := &http.Client{}
	// Prepare a form that you will submit to that URL.
	var b bytes.Buffer
	w := multipart.NewWriter(&b)
	for key, r := range values {
		var fw io.Writer

		// Add an image file
		if x, ok := r.(*os.File); ok {
			if fw, err = w.CreateFormFile(key, x.Name()); err != nil {
				return
			}
		} else {
			// Add other fields
			if fw, err = w.CreateFormField(key); err != nil {
				return
			}
		}

		if _, err = io.Copy(fw, r); err != nil {
			return err
		}

		if x, ok := r.(io.Closer); ok {
			x.Close()
		}

	}
	// Don't forget to close the multipart writer.
	// If you don't close it, your request will be missing the terminating boundary.
	err = w.Close()
	if err != nil {
		return err
	}

	// Now that you have a form, you can submit it to your handler.
	req, err := http.NewRequest("PUT", url, &b)
	if err != nil {
		return err
	}
	// Don't forget to set the content type, this will contain the boundary.
	req.Header.Set("Content-Type", w.FormDataContentType())
	req.Header.Set(ApiKeyHeader, apiKey)

	// Submit the request
	res, err := client.Do(req)
	if err != nil {
		return err
	}

	// Check the response
	if res.StatusCode != http.StatusOK {
		err = fmt.Errorf("bad status: %s", res.Status)
	}

	s, err := ioutil.ReadAll(res.Body)
	if err != nil {
		console.PrintError("Error reading response body. Error %v", err)
		return err
	}
	console.PrintLn("Response body %v", string(s))

	return
}

func mustOpen(f string) *os.File {
	r, err := os.Open(f)
	if err != nil {
		panic(err)
	}
	return r
}

func getOrgAndProject(orgAndProject string) (org string, project string, err error) {
	p := strings.Split(orgAndProject, "/")
	if len(p) != 2 {
		return "", "", fmt.Errorf("incorrect format of project parameter. Should be AAAA/BBB")
	}
	return p[0], p[1], nil
}
