# testhub-api

[![test-hub](https://api.test-hub.io/api/test-hub/projects/testhub-api/badge.svg?branch=master)](https://test-hub.io/test-hub/projects/testhub-api/runs)&nbsp;


# Contribute

## Project Structure

- TestHub.Api.Data - Rest Api data model. Only classes that are serialized to and from json in Rest interface. 
- TesthHub.Api.Test - unit tests for Eest interface 
- TestHub.Integration.Github - github integration code. Not used for now
- *TestsHub.Api* - entry point of the service. All Rest API controllers reside here 
- TestsHub.Commons - common code for all projects. Things like loggers, global constants etc.
- TestsHub.Data - data layer. Code that is responsible for data manipulation and database connectivity 
- TestsHubUploadEndpoint - test reports uploading logic. Test reports parsers reside here. The idea to make it a separate assembly was to be able to decouple it from "interface" code and scale it separately. 
- TestsHubUploadEndpoint.Test - unit-tests for reports uploading