package pkg

import (
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"
	"strings"
)

// We have to use it only when we take org name from environment and not from user
// otherwise it's really easy to workaround
func getApiKey(orgAndProject string, testhubDomain string, onPremise bool) (string, error) {

	org, _, err := getOrgAndProject(orgAndProject)
	if err != nil {
		return "", err
	}

	if isGithubActionEnv(orgAndProject) || isTravisCiEnv(orgAndProject) || isCircleCiEnv(orgAndProject) || isOnPremiseEnv(onPremise, testhubDomain) {
		log.Println("Auto-retrieval of API token")
	} else {
		return "", fmt.Errorf("Api Token must be provided as an argument")
	}

	url := fmt.Sprintf("%s/api/%s/apikey", testhubDomain, org)
	log.Printf("Requesting token url: %s", url)
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
		log.Println("Github actions environment detected")
		return true
	}
	return false
}

func isTravisCiEnv(orgAndProject string) bool {
	isTravis := os.Getenv("TRAVIS")
	user := os.Getenv("USER")
	home := os.Getenv("HOME")
	repo := os.Getenv("TRAVIS_REPO_SLUG")

	if isTravis == "true" && user == "travis" && strings.Contains(home, "travis") &&
		strings.EqualFold(repo, orgAndProject) {
		log.Println("Travis CI environment detected")
		return true
	}
	return false
}

func isCircleCiEnv(orgAndProject string) bool {
	isTravis := os.Getenv("CIRCLECI")
	circleJob := os.Getenv("CIRCLE_JOB")
	work_dir := os.Getenv("CIRCLE_WORKING_DIRECTORY")
	repoUrl := os.Getenv("CIRCLE_REPOSITORY_URL")

	if isTravis == "true" && len(circleJob) != 0 && len(work_dir) != 0 &&
		strings.Contains(strings.ToLower(orgAndProject), strings.ToLower(repoUrl)) {
		log.Println("Circle CI environment detected")
		return true
	}
	return false
}
