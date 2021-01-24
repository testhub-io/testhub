package pkg

import (
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"
	"strings"
	"testhub-cli/pkg/console"
)

// We have to use it only when we take org name from environment and not from user
// otherwise it's really easy to workaround
func getApiKey(orgAndProject string, testhubDomain string, onPremise bool) (string, error) {

	org, _, err := getOrgAndProject(orgAndProject)
	if err != nil {
		return "", err
	}

	if !isGithubActionEnv(orgAndProject) && !isOnPremiseEnv(onPremise, testhubDomain) {
		return "", fmt.Errorf("Api Token must be provided as an argument.")
	} else {
		console.PrintLn("Auto-retrieval of API token")
	}

	url := fmt.Sprintf("%s/api/%s/apikey", testhubDomain, org)

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

func isOnPremiseEnv(onPremise bool, testhubDomain string) bool {
	return onPremise && !strings.EqualFold(testhubDomain, defaultTesthubDomain)
}

func isGithubActionEnv(orgAndProject string) bool {
	isGhActions := os.Getenv("GITHUB_ACTIONS")
	ghActionsRepos := os.Getenv("GITHUB_REPOSITORY")

	if isGhActions == "true" && strings.EqualFold(ghActionsRepos, orgAndProject) {
		return true
	}
	return false
}
