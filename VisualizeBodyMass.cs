using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;
using System.Linq;
using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;


public class VisualizeBodyMass : MonoBehaviour
{
    int currentSceneID;
    string[] sceneTypes = {"forest_bodymass.json", "savanna_bodymass.json", "wetland_bodymass.json"};

    List<string> speciesNames = new List<string>();
    List<float> bodyMass = new List<float>();

    // Start is called before the first frame update
    public void Start()
    {
        currentSceneID = SceneManager.GetActiveScene().buildIndex - 1;

        DataLoader(sceneTypes[currentSceneID]);
    }

    // Update is called once per frame
    public void GetData()
    {
        currentSceneID = SceneManager.GetActiveScene().buildIndex - 1;
        //DataLoader(sceneTypes[currentSceneID]);
    }


    // Loads data that is relevant to the current visualization
    public void DataLoader(string sceneType)
    {
        using (StreamReader r = new StreamReader("Assets/"+sceneType))  
        {  
            string json = r.ReadToEnd();  
            string dataEdited = json.Replace("{", "");
            string dataEdited2 = dataEdited.Replace("}","");
            string[] data = dataEdited2.Split(',');
            Debug.Log(data.Length);
            
            for (int i = 0; i < data.Length; i ++)
            {
                string[] dataEntry = data[i].Split(":");
                speciesNames.Add(dataEntry[0]);
                bodyMass.Add(float.Parse(dataEntry[1], CultureInfo.InvariantCulture.NumberFormat));
            }
        }
    }
}

