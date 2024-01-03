using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    public static DialogueManager _instance;
    public List<TextAsset> dialogueFiles;
    public Story story;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(this);
    }
    
    public void StartDialogue(int index)
    {
        story = new Story(dialogueFiles[index].text);
        RefreshView();
    }
    
    void RefreshView()
    {
        dialogueBox.SetActive(true);
        dialogueText.text = story.Continue();
        dialogueText.text = story.Continue();
        dialogueText.text = story.Continue();
    }
    
}
