#!/bin/ash

echo "Starting test results upload..."

if [ -z "$APITOKEN" ]
then
      export ON_PREMISE=true
      echo "Pattern for test results: $TEST_FILES_PATTERN" 
      /usr/bin/testhub-cli upload -p "$REPOSITORY" -f "$TEST_FILES_PATTERN" -b "$RUN_NUMBER" -r "$WORKSPACE_DIR" --branch "$BRANCH"

      echo "Pattern for coverage: $COVERAGE_FILES_PATTERN"
      /usr/bin/testhub-cli uploadCoverage -p "$REPOSITORY" -f "$COVERAGE_FILES_PATTERN" -b "$RUN_NUMBER" -r "$WORKSPACE_DIR"
else      
      echo "Pattern for test results: $TEST_FILES_PATTERN" 
      /usr/bin/testhub-cli upload -p "$REPOSITORY" -f "$TEST_FILES_PATTERN" -b "$RUN_NUMBER" -r "$WORKSPACE_DIR" --branch "$BRANCH" -t $APITOKEN

      echo "Pattern for coverage: $COVERAGE_FILES_PATTERN"
      /usr/bin/testhub-cli uploadCoverage -p "$REPOSITORY" -f "$COVERAGE_FILES_PATTERN" -b "$RUN_NUMBER" -r "$WORKSPACE_DIR" -t $APITOKEN
fi


time=$(date)
echo "::set-output name=time::$time"

echo "Finish results upload..."