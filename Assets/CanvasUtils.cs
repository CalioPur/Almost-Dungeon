using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUtils : MonoBehaviour
{

    private void Start()
    {
        
    }

    public void ToMainMenu()
    {
        SaveSystem.SaveAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}