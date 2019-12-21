az acr login --name testhubio
docker build . -f TestsHub.Api/Dockerfile -t testhubio.azurecr.io/testhub_api
docker push  testhubio.azurecr.io/testhub_api
