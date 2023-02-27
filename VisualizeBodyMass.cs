using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;
using System.Linq;
using System;
using System.Globalization;


public class VisualizeBodyMass : MonoBehaviour
{
    int currentSceneID;
    string[] sceneTypes = {"forest.csv", "savanna.csv", "wetland.csv"};
    private readonly string url = "http://10.38.190.131:8000/download";
    
    public class PrimateData
    {
        public string habitatType;
        public string visType;
    }

    // Start is called before the first frame update
    public void Start()
    {
        currentSceneID = SceneManager.GetActiveScene().buildIndex - 1;
        // Debug.Log(sceneTypes[currentSceneID]);
        StartCoroutine(PostDataToFetch(sceneTypes[currentSceneID]));
    }

    // Update is called once per frame
    public void GetData()
    {
        currentSceneID = SceneManager.GetActiveScene().buildIndex - 1;
        // Debug.Log(sceneTypes[currentSceneID]);
        // StartCoroutine(PostDataToFetch(sceneTypes[currentSceneID]));
    }

    IEnumerator PostDataToFetch(string sceneType)
    {
        PrimateData primateDataToSend = new PrimateData();
        primateDataToSend.habitatType = sceneType;
        primateDataToSend.visType = "bodymass";

        string bodyJsonString = JsonUtility.ToJson(primateDataToSend);
        var request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("Network Error: " + request.error);
        }
        else if (request.isHttpError)
        {
            Debug.Log("Http Error: " + request.error);
        }
        else
        {
            // Extract the data sent by the server
            string receivedData = request.downloadHandler.text;
            string editedData = receivedData.Replace("[", "");
            string editedData2 = editedData.Replace("]", "");
            string[] dataAsList = editedData2.Split(",");

            // Divide data entries in dataAsList into two parallel lists, each containing primate names and body mass
            List<string> PrimateNames = new List<string>();
            List<float> BodyMass = new List<float>();
            for (int i = 0; i < dataAsList.Length-1; i+=2)
            {
                PrimateNames.Add(dataAsList[i]);
                BodyMass.Add(float.Parse(dataAsList[i+1], CultureInfo.InvariantCulture.NumberFormat));
            }
            Debug.Log(PrimateNames.Count);
            Debug.Log(BodyMass.Count);

            // To do: Spawn circles according to the sent data
        }
    }
}
