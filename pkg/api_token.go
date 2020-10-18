package pkg

import (
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
)

// We have to use it only when we take org name from environment and not from user
// otherwise it's really easy to workaround
func getApiKey(orgAndProject string) (string, error) {
	org, _, err := getOrgAndProject(orgAndProject)
	if err != nil {
		return "", err
	}
	url := fmt.Sprintf("https://%s/api/%s/apikey", testhubDomain, org)

	req, err := http.NewRequest(http.MethodGet, url, nil)
	req.Header.Set("token", "WinLost2020$")
	if err != nil {
		return "", err
	}
	client := &http.Client{}
	res, err := client.Do(req)

	k, err := ioutil.ReadAll(res.Body)
	res.Body.Close()
	if err != nil {
		log.Fatal(err)
	}
	return string(k), nil
}
