# Testhub

Testhub is a tests results aggregator that makes it easy to upload tests results from CI pipeline and present them in a visual form, provides insightful analytics.

|Organiaztion View | Project View | Test-run View | Testgrid |
|-|-|-|-|
|![Organiaztion](docs/images/screenshot1.png) | ![Project](docs/images/screenshot2.png) |![Test-run](docs/images/screenshot3.png)| ![Testgrid](docs/images/screenshot4.png)

Jump to a demo project and play with interface [https://test-hub.io/testhub-io-examples](https://test-hub.io/testhub-io-examples) 



## 👷 Build Status

[![Frontend](https://github.com/testhub-io/testhub/actions/workflows/frontend.yml/badge.svg)](https://github.com/testhub-io/testhub/actions/workflows/frontend.yml)
[![Backend](https://github.com/testhub-io/testhub/actions/workflows/backend.yml/badge.svg)](https://github.com/testhub-io/testhub/actions/workflows/backend.yml)
[![test-hub](https://api.test-hub.io/api/test-hub/projects/testhub-api/badge.svg?branch=master)](https://test-hub.io/test-hub/projects/testhub-api/runs)&nbsp;


## 🚀 Getting Started

### Managed Testhub 
Testhub have managed version that is hosted on https://test-hub.io. It's free for open source projects. Follow below guide to upload your test results

Check out our [getting started guide](https://testhub-io.github.io/testhub/docs/) to onboatd you project in 5 minutes 

### Self-hosted

#### Helm Charts 
Easiest way to deploy Testhub in Kuberneted cluster in helm-chart package
Here is a guide on how to do it: https://testhub-io.github.io/testhub-helm/

#### Docker 
Dockerhub we "fresh" images: https://hub.docker.com/u/testhubio
Check the [docker-compose.yml](/docker-compose.yaml) to see how to run them togeather 
> Please note that there are two dockerfiles one for "[genera purpose](/frontend/Dockerfile)" and one for [self hosted setup](/frontend/Dockerfile-on-prem.dockerfile). In latter, we override `API_ENDPOINT` env var to make frontend work with hosted backend. Those images have `on-prem` in image tag on dockerhub.

## 👏 Contributing
We love help! Contribute by forking the repo and opening pull requests. Please ensure that your code passes the existing tests and linting, and write tests to test your changes if applicable.

All pull requests should be submitted to the main branch.
