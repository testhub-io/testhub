/*
Copyright © 2020 NAME HERE <EMAIL ADDRESS>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
package cmd

import (
	"github.com/fatih/color"
	"github.com/spf13/cobra"
	"os"
	"testhub-cli/pkg"
)

// uploadCmd represents the upload command
var uploadCmd = &cobra.Command{
	Use:   "upload",
	Short: "Upload test results",
	Long:  `Search for test results files and upload them as a Test Results`,
	Run: func(cmd *cobra.Command, args []string) {
		err := uploadParams.UploadTestResultFiles()
		if err != nil {
			color.Red("Error executing upload. Err: %v", err)
			os.Exit(1)
		}
	},
}

// uploadCmd represents the upload command
var uploadCoverageCmd = &cobra.Command{
	Use:   "uploadCoverage",
	Short: "Upload test coverage",
	Long:  `Search for test results files and upload them as a Test Results`,
	Run: func(cmd *cobra.Command, args []string) {
		uploadParams.IsCoverage = true
		err := uploadParams.UploadTestResultFiles()
		if err != nil {
			color.Red("Error executing upload. Err: %v", err)
			os.Exit(1)
		}
	},
}

var uploadParams = new(pkg.UploadFilesParameters)

func init() {
	rootCmd.AddCommand(uploadCmd, uploadCoverageCmd)

	addCommonFlags(uploadCmd)
	addCommonFlags(uploadCoverageCmd)
}

func addCommonFlags(cmd *cobra.Command) {
	cmd.Flags().StringVarP(&uploadParams.ApiToken, "ApiToken", "t", "", "Api token")
	cmd.MarkFlagRequired("ApiToken")

	cmd.Flags().StringVarP(&uploadParams.OrgAndProject, "project", "p", "", "Organisation and Project name separated by slash like test-org/repo_a")
	cmd.MarkFlagRequired("project")

	const build = "build"
	cmd.Flags().StringVarP(&uploadParams.Build, build, "b", "", "Build name or id")
	cmd.MarkFlagRequired(build)

	const pattern = "pattern"
	cmd.Flags().StringVarP(&uploadParams.FilePattern, pattern, "f", "", "Files pattern to search and upload")
	cmd.MarkFlagRequired(pattern)

	cmd.Flags().BoolVarP(&uploadParams.IsTestRun, "debug", "d", false, "Skip uploading and test the pattern only")

	cmd.Flags().StringVarP(&uploadParams.ContextDir, "root", "r", "", "Files pattern to search and upload")

	cmd.Flags().StringVarP(&uploadParams.Branch, "branch", "", "not specified", "Branch")
}
