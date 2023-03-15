using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowVisualizationPage : MonoBehaviour
{
   int currentSceneID;
   int visSceneID;

   public void Load()
    {
        currentSceneID = SceneManager.GetActiveScene().buildIndex;
        visSceneID = currentSceneID + 3;
        SceneManager.LoadScene(visSceneID);
    }
}
