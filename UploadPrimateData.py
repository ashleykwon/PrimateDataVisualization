import uvicorn
from fastapi import FastAPI
import os 
import numpy as np
from PIL import Image
from pydantic import BaseModel
import json
import csv

app = FastAPI()

class Input(BaseModel):
    habitatType: str
    visType: str

# Downloads the JSON file sent by a Unity, fetches appropriate data, and sends it back to the Unity project
@app.put("/download")
def dataDownloader(d:Input):
    habitatType = d.habitatType
    visType = d.visType
    dataToSend = dataUploader(visType, habitatType)
    return dataToSend


# Fetches requested data and processes it for the Unity project to retrieve
def dataUploader(visType, habitatType):
    dataToSend = []
    
    # Open the chosen CSV file
    dataDir = 'path'
    fileToRead = dataDir+habitatType
    lineNum = 0

    # Iterate through the csv file and extracts requested data
    with open(fileToRead, 'r') as csv_file:
        csv_reader = csv.reader(csv_file)
        for line in csv_reader:
            if lineNum != 0: #don't include metadata
                # dataEntry = [line[2]] # append common name regardless of visualization type
                dataToSend.append(line[2])
                if visType == "bodymass":
                    dataToSend.append(float(line[5])) #append body mass
                elif visType == "diet":
                    dataToSend.append(line[4])
                else:
                    print("error")
                # dataToSend.append(dataEntry)
            lineNum += 1
    return dataToSend




if __name__ == '__main__':
    uvicorn.run(app, host='ip-address', port=8000) # change the host name
    # print(dataUploader())