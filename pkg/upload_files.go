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
	FilePattern   string
	OrgAndProject string
	Build         string
	ContextDir    string
	Branch        string
	IsCoverage    bool
	IsTestRun     bool
	ApiToken      string
}

const testhubDomain = "test-hub-api.azurewebsites.net"
const ApiKeyHeader = "ApiToken"

func (u *UploadFilesParameters) UploadTestResultFiles() error {
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

	return nil
}

func (u *UploadFilesParameters) uploadFile(f string, isCoverage bool) error {

	org, proj, err := getOrgAndProject(u.OrgAndProject)
	if err != nil {
		return err
	}
	url := fmt.Sprintf("https://%s/api/%s/projects/%s/runs/%s", testhubDomain, org, proj, u.Build)

	fileParamName := "testResult"
	if isCoverage {
		file := mustOpen(f)
		err := uploadCoverage(org, proj, u.Build, file, u.ApiToken)
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

func uploadCoverage(org string, proj string, build string, file *os.File, apiKey string) error {
	url := fmt.Sprintf("https://%s/api/%s/projects/%s/runs/%s/coverage", testhubDomain, org, proj, build)

	req, err := http.NewRequest(http.MethodPut, url, file)
	if err != nil {
		return errors.Wrap(err, "Error creating http request")
	}

	req.Header.Set(ApiKeyHeader, apiKey)
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
