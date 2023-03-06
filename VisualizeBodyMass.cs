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
using System.Threading.Tasks;


public class VisualizeBodyMass : MonoBehaviour
{
    int currentSceneID;
    string[] sceneTypes = {"forest_bodymass", "savanna_bodymass", "wetland_bodymass"};

    List<string> speciesNames = new List<string>();
    List<float> bodyMass = new List<float>();

    public GameObject PrimateSphere;

    // Start is called before the first frame update
    public void Start()
    {
        currentSceneID = SceneManager.GetActiveScene().buildIndex - 4;
        
        // DataLoader(sceneTypes[currentSceneID]);
        // SphereGenerator(speciesNames, bodyMass);
    }

    // Update is called once per frame
    public void GetData()
    {
        currentSceneID = SceneManager.GetActiveScene().buildIndex - 4;
        Debug.Log("Button pressed");
        DataLoader(sceneTypes[currentSceneID]);
        SphereGenerator(speciesNames, bodyMass);
        Debug.Log("executed");
    }



    // Loads data that is relevant to the current visualization
    public void DataLoader(string sceneType)
    {     
        string json = Resources.Load<TextAsset>("Json/"+sceneType).text;
        Debug.Log(json);

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

    public void SphereGenerator(List<string> speciesNames, List<float> bodyMass)
    {
        bodyMass.Sort();
        for (int i = 0; i < speciesNames.Count; i++)
        {
            // GameObject generatedPrimate = Instantiate(PrimateSphere, new Vector3(0, 0, 300), Quaternion.identity);
            // Transform primateSphere = transform.Find("DefaultSphere");
            // Debug.Log(primateSphere);
            GameObject primateSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // if (i < speciesNames.Count/2)
            // {
            //     primateSphere.transform.position = new Vector3(-1*i*10, 0, 500);
            // }
            primateSphere.transform.position = new Vector3(i*30, 0, 100);
            primateSphere.transform.localScale = new Vector3(bodyMass[i]*100, bodyMass[i]*100, bodyMass[i]*100);
        }
    }
}

