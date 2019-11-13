import urllib2
import re
import time




response = urllib2.urlopen('https://gcsweb.k8s.io/gcs/kubernetes-jenkins/logs/ci-kubernetes-e2e-gci-gke-serial/')
html = response.read()
res =  re.findall('ci-kubernetes-e2e-gci-gke-serial\/.*\/\"',html)

# https://storage.googleapis.com/kubernetes-jenkins/logs/ci-kubernetes-e2e-gci-gke-serial/1169263880568836097/artifacts/junit_runner.xml
# ci-kubernetes-e2e-gci-gke-serial/1161386049956483072/"

# https://storage.googleapis.com/kubernetes-jenkins/logs/ci-kubernetes-e2e-gci-gke-serial/1169263880568836097/artifacts/junit_runner.xml

for l in res:
    try:
        l = l.rstrip('"')
        url = 'https://storage.googleapis.com/kubernetes-jenkins/logs/%sartifacts/junit_runner.xml' % l
        print (url)
        response = urllib2.urlopen(url)

        fileName = l.replace('ci-kubernetes-e2e-gci-gke-serial/', '').rstrip('/') 
        text_file = open(fileName, "w")
        text_file.write(response.read())
        text_file.close()        
        print (fileName)
        time.sleep(1)
    except Exception as e:
        print("An exception occurred:" + str(e))

 