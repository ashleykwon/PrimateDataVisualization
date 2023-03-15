using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHome : MonoBehaviour
{
    public void Update()
    {
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        Debug.Log("trigger right");
        Debug.Log(triggerRight);

        if (triggerRight > 0.9f)
        {
            Load();
        }
    }
    
    public void Load()
    {
        SceneManager.LoadScene(0);
    }
}