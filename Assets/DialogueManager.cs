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
    private GameObject choice1;
    private GameObject choice2;
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
        choice1 = GameObject.Find("Choice1");
        choice2 = GameObject.Find("Choice2");
        dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>();
        nextButton = dialogueBox.GetComponentInChildren<Button>();
        nextButton.onClick.AddListener(NextDialogue);
    }
    public void StartDialogue(int indexDg, int indexLv)
    {
        GetUiElements();

        if (indexDg >= dialogues.Count || indexLv >= dialogues[indexDg].dialogueFiles.Count)
        {
            dialogueBox.SetActive(false);
            return;
        }

        try
        {
            story = new Story(dialogues[indexDg].dialogueFiles[indexLv].text);
        }
        catch (Exception e){
            dialogueBox.SetActive(false);
            Time.timeScale = 1;
            return; 
        }
        
        
        dialogueBox.SetActive(true);
        choice1.SetActive(false);
        choice2.SetActive(false);
        RefreshView();
        Time.timeScale = 0;
    }

    public void NextDialogue()
    {
        RefreshView();
    }

    void RefreshView()
    {
        var choices = story.currentChoices.Count;
        
        
        
        if (story.canContinue)
        {
            dialogueText.text = story.Continue();
            if (story.currentTags[0].Contains("knight")) ShowArrowKnight();
            else ShowArrowDragon();
            
            
        }
        else
        {
            if(choices > 0)
            {
                DisplayChoices();
                return;
            }
            dialogueBox.SetActive(false);
            nextButton.onClick.RemoveAllListeners();
            story = null;
            Time.timeScale = 1;
        }
    }

    private void DisplayChoices()
    {
        DisplayChoices(story.currentChoices);
    }

    private void DisplayChoices(List<Choice> storyCurrentChoices)
    {
        choice1.SetActive(true);
        choice2.SetActive(true);
        nextButton.gameObject.SetActive(false);
        choice1.GetComponentInChildren<TMP_Text>().text = storyCurrentChoices[0].text;
        choice2.GetComponentInChildren<TMP_Text>().text = storyCurrentChoices[1].text;
        choice1.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickChoiceButton(storyCurrentChoices[0]); });
        choice2.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickChoiceButton(storyCurrentChoices[1]); });
    }
    
    void OnClickChoiceButton (Choice choice) {
        story.ChooseChoiceIndex (choice.index);
        choice1.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        choice2.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        choice1.SetActive(false);
        choice2.SetActive(false);
        nextButton.gameObject.SetActive(true);
        RefreshView();
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
