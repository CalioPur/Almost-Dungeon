using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public struct DungeonDialogue
{
    [SerializeField] public List<TextAsset> dialogueFiles;
}

public class DialogueManager : MonoBehaviour
{
    
    public static DialogueManager _instance;
    public List<DungeonDialogue> dialogues;
    private Story story;
    private GameObject dialogueBox;
    private GameObject arrowKnight;
    private GameObject arrowDragon; 
    private TMP_Text dialogueText;
    private Button nextButton;
    
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
    
    private void GetUiElements()
    {
        dialogueBox = GameObject.Find("DialoguePanel");
        arrowKnight = GameObject.Find("ArrowKnight");
        arrowDragon = GameObject.Find("ArrowDragon");
        dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>();
        nextButton = dialogueBox.GetComponentInChildren<Button>();
        nextButton.onClick.AddListener(NextDialogue);
    }
    public void StartDialogue(int indexDg, int indexLv)
    {
        
        if(indexDg >= dialogues.Count || indexLv >= dialogues[indexDg].dialogueFiles.Count) return; //evite les erreurs
        story = new Story(dialogues[indexDg].dialogueFiles[indexLv].text);
        if(story == null) return; //evite les erreurs
        
        GetUiElements();
        dialogueBox.SetActive(true);
        
        RefreshView();
        Time.timeScale = 0;
    }

    public void NextDialogue()
    {
        RefreshView();
    }

    void RefreshView()
    {
        if (story.canContinue)
        {
            dialogueText.text = story.Continue();
            if (story.currentTags[0].Contains("knight")) ShowArrowKnight();
            else ShowArrowDragon();
        }
        else
        {
            dialogueBox.SetActive(false);
            nextButton.onClick.RemoveAllListeners();
            Time.timeScale = 1;
        }
    }
    
    void ShowArrowKnight()
    {
        arrowDragon.SetActive(false);
        arrowKnight.SetActive(true);
    }
    
    void ShowArrowDragon()
    {
        arrowDragon.SetActive(true);
        arrowKnight.SetActive(false);
    }
    
}
