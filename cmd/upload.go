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
	Short: "A brief description of your command",
	Long: `A longer description that spans multiple lines and likely contains examples
and usage of using your command. For example:

Cobra is a CLI library for Go that empowers applications.
This application is a tool to generate the needed files
to quickly create a Cobra application.`,
	Run: func(cmd *cobra.Command, args []string) {
		err := uploadParams.UploadTestResultFiles()
		if err != nil{
			color.Red("Error executing upload. Err: %v", err)
			os.Exit(1)
		}
	},
}

var uploadParams = new(pkg.UploadFilesParameters)

func init() {
	rootCmd.AddCommand(uploadCmd)

	const orgCommand = "org"
	uploadCmd.Flags().StringVarP(&uploadParams.Org, orgCommand, "o", "", "Organisation name")
	uploadCmd.MarkFlagRequired(orgCommand)

	uploadCmd.Flags().StringVarP(&uploadParams.Project, "project", "p", "", "Project name")

	const build = "build"
	uploadCmd.Flags().StringVarP(&uploadParams.Build, build, "b", "", "Build name or id")
	uploadCmd.MarkFlagRequired(build)

	const pattern = "pattern"
	uploadCmd.Flags().StringVarP(&uploadParams.FilePattern, pattern, "f", "", "Files pattern to search and upload")
	uploadCmd.MarkFlagRequired(pattern)

	uploadCmd.Flags().BoolVarP(&uploadParams.IsTestRun,"debug", "d", false, "Skip uploading and test the pattern only")


	uploadCmd.Flags().StringVarP(&uploadParams.ContextDir, "root", "r", "", "Files pattern to search and upload")

}
