using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUtils : MonoBehaviour
{
    [SerializeField] SaveSystem saveSystem;

    private void Start()
    {
        saveSystem = new SaveSystem();
    }

    public void ToMainMenu()
    {
        SaveSystem.SaveAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
