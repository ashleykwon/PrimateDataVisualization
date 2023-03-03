import numpy as np
from PIL import Image
import json
import csv

# Generates JSON files of primate data for a C# script to read
def dataUploader(visType, habitatType):
    dataToSend = {}
    
    # Open the chosen CSV file
    dataDir = 'datadir'
    fileToRead = dataDir+habitatType
    lineNum = 0

    # Iterate through the csv file and extracts requested data
    with open(fileToRead, 'r') as csv_file:
        csv_reader = csv.reader(csv_file)
        for line in csv_reader:
            if lineNum != 0: #don't include metadata
                # dataEntry = [line[2]] 
                if visType == "bodymass":
                    dataToSend[line[2]] = float(line[5]) #append body mass
                elif visType == "diet":
                    dataToSend[line[2]] =line[4]
                else:
                    print("error")
                # dataToSend.append(dataEntry)
                # dataToSend[int(lineNum)] = dataEntry
            lineNum += 1
    with open("savanna_bodymass.json", "w") as fileName:
        json.dump(dataToSend, fileName)
    return dataToSend




if __name__ == '__main__':
    print(dataUploader("bodymass", "savanna.csv"))