package main

import (
	"bytes"
	"fmt"
	"github.com/fatih/color"
	"github.com/pkg/errors"
	"io"
	"mime/multipart"
	"net/http"
	"os"
	"path/filepath"
	"strings"
)

type UploadFilesParameters struct {
	testPatter string
	org        string
	project    string
	build      string
	contextDir string
	isCoverage bool
}


func (u *UploadFilesParameters) UploadTestResultFiles () error{
	root := u.contextDir
	if len(u.contextDir) == 0{
		root = "."
	}

	files, err := walkMatch(root, u.testPatter)
	if err != nil{
		return errors.Wrap(err, "Error listing the files")
	}

	for _,f := range (files){
		err = uploadFile(f, u.isCoverage)
		if err != nil{
			color.Red("Fail to upload file %s. Error: %v", f, err)
		}
	}

	return nil
}

func uploadFile(f string, isCoverage bool) error {
	const testhubDomain = "test-hub-api.azurewebsites.net"
	url := fmt.Sprintf("https://%s/api/%s/projects/%s/runs/%s",testhubDomain,  u.org, u.project, u.build)

	values := map[string]io.Reader{
		"testResult":mustOpen(f),
		"branch": strings.NewReader("not specified"),
	}
	err := upload(url, values )
	return err
}


func upload( url string, values map[string]io.Reader) (err error) {
	client := &http.Client{}
	// Prepare a form that you will submit to that URL.
	var b bytes.Buffer
	w := multipart.NewWriter(&b)
	for key, r := range values {
		var fw io.Writer
		if x, ok := r.(io.Closer); ok {
			defer x.Close()
		}
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

	}
	// Don't forget to close the multipart writer.
	// If you don't close it, your request will be missing the terminating boundary.
	w.Close()

	// Now that you have a form, you can submit it to your handler.
	req, err := http.NewRequest("POST", url, &b)
	if err != nil {
		return
	}
	// Don't forget to set the content type, this will contain the boundary.
	req.Header.Set("Content-Type", w.FormDataContentType())

	// Submit the request
	res, err := client.Do(req)
	if err != nil {
		return
	}

	// Check the response
	if res.StatusCode != http.StatusOK {
		err = fmt.Errorf("bad status: %s", res.Status)
	}
	return
}

func mustOpen(f string) *os.File {
	r, err := os.Open(f)
	if err != nil {
		panic(err)
	}
	return r
}



func walkMatch(root, pattern string) ([]string, error) {
	var matches []string
	err := filepath.Walk(root, func(path string, info os.FileInfo, err error) error {
		if err != nil {
			return err
		}
		if info.IsDir() {
			return nil
		}
		if matched, err := filepath.Match(pattern, filepath.Base(path)); err != nil {
			return err
		} else if matched {
			matches = append(matches, path)
		}
		return nil
	})
	if err != nil {
		return nil, err
	}
	return matches, nil
}