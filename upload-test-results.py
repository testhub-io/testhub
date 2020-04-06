import requests
import glob
import argparse
import json
from pathlib import Path


import os
import os.path

#parse arguments
parser = argparse.ArgumentParser()
parser.add_argument("--file", "-f", help="Test file pattern")
parser.add_argument("--org", "-o", help="Organisation")
parser.add_argument("--project", "-p", help="Project name")
parser.add_argument("--build", "-b", help="Build id")


args = parser.parse_args()
print(args.file)

for root, dirs, files in os.walk("."):
    for file in files:        
        if file.find(args.file) != -1:           
            print(os.path.join(root, file))
            print(root.split("/")[1])
            testRunId = root.split("/")[1]
            
            url = "https://test-hub-api.azurewebsites.net/api/{}/projects/{}/runs/{}".format(args.org, args.project, args.build)
            print("Uploading to:" + url)
            
            f=open(os.path.join(root, file))
            content=f.read()
            f.close()

            response = requests.put(url,
                        files=dict(file=content),
                        verify=False)
                        
            print("Response:" + json.dumps(response.json()))
            if response.status_code != 200:
                #exit(1)
                print("Failed to upload file")
            else:
                print("File uploaded successufully.")


            

        