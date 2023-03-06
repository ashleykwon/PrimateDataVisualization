using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHome : MonoBehaviour
{
    
    public void Load()
    {
        SceneManager.LoadScene(0);
    }
}