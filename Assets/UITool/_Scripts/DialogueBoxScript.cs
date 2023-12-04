using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DialogueBoxScript : MonoBehaviour
{
    public static event Action OnDialogueNext;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnDialogueNext?.Invoke();
        }
    }
}
