import requests
import glob
import argparse
import json
from pathlib import Path


import os
import os.path

def getCoverage (dir, coveragePatter):
    for root, dirs, files in os.walk(dir):
        for file in files:        
            if file.find(coveragePatter) != -1:        
                print("--- Coverage: " + os.path.join(root, file))
                return os.path.join(root, file)
    return ""
    
def readFileContent (filePath):
    f=open(filePath)
    content=f.read()
    f.close()
    return content

#parse arguments
parser = argparse.ArgumentParser()
parser.add_argument("--file", "-f", help="Test file pattern")
parser.add_argument("--coverage", "-c", help="Coverage file pattern")
parser.add_argument("--org", "-o", help="Organisation")
parser.add_argument("--project", "-p", help="Project name")
parser.add_argument("--build", "-b", help="Build id")


args = parser.parse_args()
print(args.file)

for root, dirs, files in os.walk("."):
    for file in files:        
        if file.find(args.file) != -1:                                   
            print("Reading test results file:" +  os.path.join(root, file))            
            print(root.split("/")[1])
            testRunId = root.split("/")[1]
                        
            url = "https://test-hub-api.azurewebsites.net/api/{}/projects/{}/runs/{}".format(args.org, args.project, args.build)
            print("Uploading to: " + url)
                                    
            toUpload = dict(file=readFileContent(os.path.join(root, file)))

            coverPath = getCoverage(root, args.coverage) 
            if coverPath != "":
                print("Uploading coverage from : " +coverPath)
                toUpload["coverage"]= readFileContent(coverPath)
            
            response = requests.put(url,
                         files=toUpload,
                         verify=False)
                                    
            if response.status_code != 200:
                #exit(1)
                print("Failed to upload file")
                print(response.text)
            else:
                print("File uploaded successufully.")
                print(response.text)


