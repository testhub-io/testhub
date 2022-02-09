# testhub-helm
Helm chart repo for TestHub

# Usage

Create a `values.yaml` file:

```
ingress:  
  frontend:
    annotations: 
      # kubernetes.io/ingress.class: nginx
      # kubernetes.io/tls-acme: "true"
    host: testhub-frontend.local    
  api:
    host: testhub-api.local    
    
volume:
  size: 1Gi
```

Add testhub helm repo and install chart

```
helm repo add testhub https://testhub-io.github.io/testhub-helm
helm repo update
helm install testhub testhub/testhub -n test-hub -f values.yaml
```

## Configure tests upload

- Install testhub-cli. Refer to https://github.com/testhub-io/testhub-cli for more info and most recent version.
```
 curl  https://github.com/testhub-io/testhub-cli/releases/download/v0.12e/testhub-cli-v0.12e-linux-386.tar.gz --output testhub-cli.tar.gz -L && tar -xzf testhub-cli.tar.gz
```

- Initialize testhub environment variables 
```
    export ON_PREMISE=true    
    export TESTHUB_DOMAIN=testhub-api.my-cluster.com
```
last one should point to backend

- Run testhub-cli after test execution 
```
 ./testhub-cli upload --build $BUILD_NUMBER --project test-project/project-x --pattern ./report.xml
```

after execution test will be available in testhub frontend under `test-project` 


