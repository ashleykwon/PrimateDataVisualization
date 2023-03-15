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
using OVR;

public class VisualizeBodyMass : MonoBehaviour
{
    int currentSceneID;
    string[] sceneTypes = {"forest_bodymass", "savanna_bodymass", "wetland_bodymass"};
    string[] dietTypes = {"forest_diet", "savanna_diet", "wetland_diet"};

    List<string> speciesNames = new List<string>();
    List<float> bodyMass = new List<float>();
    List<string> diets = new List<string>();

    public Material defaultSphereColor;

    public TMP_Text primateNameTemplate;

    public GameObject primate;

    // Start is called before the first frame update
    public void Start()
    {
        
        currentSceneID = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneID != 4)
        {
            currentSceneID -= 4;
        }
        DataLoader(sceneTypes[currentSceneID], dietTypes[currentSceneID]);
        GetData();
      
        Debug.Log(primate.GetComponent<Renderer>().material.color);
    }

    // public void Update()
    // {
    //     bool triggerRight = OVRInput.Get(OVRInput.Button.One);
    //     Debug.Log("trigger right");
    //     Debug.Log(triggerRight);

    //     if (triggerRight)
    //     {
    //         GetData();
    //     }
    // }

    public void GetData()
    {
        // Destroy all primate sphere objects that were previously in the scene
        if (DoesTagExist("primateSphere"))
        {
            GameObject[] deadPrimates = GameObject.FindGameObjectsWithTag("primateSphere");
            for (int j = 0; j < deadPrimates.Length; j++)
            {
                Destroy(deadPrimates[j]);
            }
        }

        // Load data
        currentSceneID = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneID != 4)
        {
            currentSceneID -= 4;
        }
        SphereGenerator(speciesNames, bodyMass);
    }



    // Loads data that is relevant to the current visualization
    public void DataLoader(string sceneType, string dietType)
    {     
        // Load and clean body mass data
        string json = Resources.Load<TextAsset>("Json/"+sceneType).text;
        string dataEdited = json.Replace("{", "");
        string dataEdited2 = dataEdited.Replace("}","");
        string[] data = dataEdited2.Split(',');

        // Load and clean diet data
        string json2 = Resources.Load<TextAsset>("Json/"+dietType).text;
        string dietDataEdited = json2.Replace("{", "");
        string dietDataEdited2 = dietDataEdited.Replace("}","");
        string dietDataEdited3 = dietDataEdited2.Replace(" ", "");
        string[] dietData = dietDataEdited3.Split(',');
        
        for (int i = 0; i < data.Length; i ++)
        {
            string[] dataEntry = data[i].Split(":");
            speciesNames.Add(dataEntry[0]);
            bodyMass.Add(float.Parse(dataEntry[1], CultureInfo.InvariantCulture.NumberFormat));
            diets.Add(dietData[i].Split(":")[1]);
        }

        for (int j = 0; j < diets.Count; j++)
        {
            string newStr = "";
            for (int i = 1; i < diets[j].Trim().Length-1; i++)
            {
                newStr += diets[j][i];
            }
            diets[j] = newStr;
        }
        
    }

    public void SphereGenerator(List<string> speciesNames, List<float> bodyMass)
    {
        bodyMass.Sort();
        // List<float> bodyMassReversed = bodyMass.Reverse();
        // float bodyMassSum = 0;
        float Insectivore_bodyMassSum = 0;
        float Folivore_bodyMassSum = 0;
        float Frugivore_bodyMassSum = 0;
        float Folivore_frugivore_bodyMassSum = 0;
        float Omnivore_bodyMassSum = 0;
        float Gummivore_bodyMassSum = 0;

        float Insectivore_count = 0;
        float Folivore_count = 0;
        float Frugivore_count = 0;
        float Folivore_frugivore_count = 0;
        float Omnivore_count = 0;
        float Gummivore_count = 0;
        string[] dietTags = {"Frugivore", "Folivore", "Folivore_frugivore", "Omnivore", "Gummivore", "Insectivore"};

        // Spawn diet names
        for (int j = 0; j < dietTags.Length; j++)
        {
            TMP_Text dietName = Instantiate(primateNameTemplate);
            dietName.text = dietTags[j];
            dietName.transform.position = new Vector3(-500f + 250f*j, -200f, 300f);
        }

        // TextMeshPro primateName = gameObject.AddComponent<TextMeshPro>();
        // Spawn new primate sphere objects
        for (int i = speciesNames.Count-1; i >= 0; i--)
        {
            GameObject primateSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            primateSphere.tag = "primateSphere";

            // Assign species name
            TMP_Text primateName = Instantiate(primateNameTemplate);
            primateName.text = speciesNames[i];
            primateName.color =  new Color(1f, 1f, 1f, 1f);
            

            // Assign size to primateSphere based on bodyMass
            primateSphere.transform.localScale = new Vector3(bodyMass[i]*10, bodyMass[i]*10, bodyMass[i]*10);
            

            // Assign positions and colors to primageSphere based on diet type
            primateSphere.GetComponent<Renderer>().material = defaultSphereColor; // Red for insectivores
            if (String.Equals(diets[i],"Frugivore"))
            {
                float yIdx = Frugivore_bodyMassSum + 5*Frugivore_count;
                primateSphere.transform.position = new Vector3(-500f, yIdx, 300f);
                Frugivore_bodyMassSum += bodyMass[i]*10;
                Frugivore_count += 1;

                primateName.transform.position  = new Vector3(-500f, yIdx, 300f-bodyMass[i]*10);

                primateSphere.GetComponent<Renderer>().material.color = new Color(0f, 1f, 0f, 1f); 
            }
            else if (String.Equals(diets[i],"Folivore"))
            {
                float yIdx = Folivore_bodyMassSum + Folivore_count*5;
                primateSphere.transform.position = new Vector3(-250f, yIdx, 300f);
                Folivore_bodyMassSum += bodyMass[i]*10;
                Folivore_count += 1;

                primateName.transform.position  = new Vector3(-250f, yIdx, 300f-bodyMass[i]*10);

                primateSphere.GetComponent<Renderer>().material.color = new Color(0f, 0f, 1f, 1f);
            }
            else if (String.Equals(diets[i],"Folivore_frugivore"))
            {
                float yIdx = Folivore_frugivore_bodyMassSum + Folivore_frugivore_count*5;
                primateSphere.transform.position = new Vector3(0f, yIdx, 300f);
                Folivore_frugivore_bodyMassSum += bodyMass[i]*10;
                Folivore_frugivore_count += 1;

                primateName.transform.position  = new Vector3(0f, yIdx, 300f-bodyMass[i]*10);

                primateSphere.GetComponent<Renderer>().material.color = new Color(0f, 1f, 1f, 1f);
            }
            else if (String.Equals(diets[i], "Omnivore"))
            {
                float yIdx = Omnivore_bodyMassSum + Omnivore_count*5;
                primateSphere.transform.position = new Vector3(250f, yIdx, 300f);
                Omnivore_bodyMassSum += bodyMass[i]*10;
                Omnivore_count += 1;

                primateName.transform.position  = new Vector3(250f, yIdx, 300f-bodyMass[i]*10); 

                primateSphere.GetComponent<Renderer>().material.color = new Color(1f, 0f, 1f, 1f);
            }
            else if (String.Equals(diets[i], "Gummivore"))
            {
                float yIdx = Gummivore_bodyMassSum + Gummivore_count*5;
                primateSphere.transform.position = new Vector3(500f, yIdx, 300f);
                Gummivore_bodyMassSum += bodyMass[i]*10;
                Gummivore_count += 1;

                primateName.transform.position  = new Vector3(500f, yIdx, 300f-bodyMass[i]*10);

                primateSphere.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                float yIdx = Insectivore_bodyMassSum + Insectivore_count*5;
                primateSphere.transform.position = new Vector3(750f, yIdx, 300f);
                Insectivore_bodyMassSum += bodyMass[i]*10;
                Insectivore_count += 1;

                primateName.transform.position  = new Vector3(750f, yIdx, 300f-bodyMass[i]*10);
            }

        }

        GameObject[] deadPrimates = GameObject.FindGameObjectsWithTag("primateSphere");

        // Debug.Log("spawnedSpheres length");
        // Debug.Log(deadPrimates.Length);
    }

    public static bool DoesTagExist(string aTag)
    {
        try
        {
        GameObject.FindGameObjectsWithTag(aTag);
        return true;
        }
        catch
        {
        return false;
        }
    }

    // public static float primateYPosition(float bodyMass, float bodyMassSum, float primateCount)
    // {
    //     float yPos = bodyMassSum + primateCount*5;
    //     if (bodyMass < 111.0f)
    //     {
    //         yPos = bodyMassSum + primateCount*5+ 111.0f;
    //     }
    //     return yPos;
    // }
}

