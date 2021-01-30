# testhub-cli


[![Gitpod ready-to-code](https://img.shields.io/badge/Gitpod-ready--to--code-blue?logo=gitpod)](https://gitpod.io/#https://github.com/testhub-io/testhub-cli)

# Usage 

`$GITHUB_REPOSITORY`  -  organisation and reposirory. Example `test-org/java-project`
`$INPUT_TEST_RESULT_PATTERN` - glob patter to search for test results files. Example `/target/surefire-reports/**/*.xml`
`$BUILD_NUMBER` - bulid number
`$WORKING_DIR` - directory where to start search for test files. (optional)
`$BRANCH` - branch name
`$INPUT_TEST_COVERAGE_PATTERN` -  glob patter to search for coverage files


## Upload test results
```
testhub-cli upload -p $GITHUB_REPOSITORY -f "$INPUT_TEST_RESULT_PATTERN" -b $BUILD_NUMBER -r "$WORKING_DIR" --branch "$BRANCH"
```

## Upload coverage
```
testhub-cli uploadCoverage -p "$GITHUB_REPOSITORY" -f "$INPUT_TEST_COVERAGE_PATTERN" -b $BUILD_NUMBER -r "$WORKING_DIR"
```


# Configuration 
`TESTHUB_DOMAIN` - testhub api domain (test-hub-api.azurewebsites.net is default)

`ON_PREMISE` - set to 'true' if it runs in on premise configuration

