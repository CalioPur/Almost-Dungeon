using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour
{
    [SerializeField] GameObject interactText;
    [SerializeField] UIDocument interactTextUIDocument;
    [SerializeField] GameObject dialogueUI;
    [SerializeField] string identifier;
    List<string> interactTextString;
    
    [SerializeField] UIDocument DialogueUiDocument;
    
    bool isPlayerNear = false;
    bool inDialogue = false;
    int textIndex = 0;
    
    void Start()
    {
        UpdateDialogue();
        UiManagerInGame.OnLanguageChange += UpdateDialogue;
    }

    private void OnDisable()
    {
        UiManagerInGame.OnLanguageChange -= UpdateDialogue;
    }

    private void UpdateDialogue()
    {
        interactTextString = XMLReader.languages[XMLReader.currentLanguage][identifier].Split('\n').ToList();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactText.SetActive(true);
            //change text of the interact text
            var interactText_root_element = interactTextUIDocument.rootVisualElement;
            var text = interactText_root_element.Q<Label>("text");
            text.text = XMLReader.languages[XMLReader.currentLanguage]["interaction"];
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E)&& !inDialogue)
        {
            //interactText.SetActive(false);
            Time.timeScale = 0;
            dialogueUI.SetActive(true);
            initDialogueUi();
            DialogueBoxScript.OnDialogueNext += changeDialogue;
            inDialogue = true;
        }
    }
    
    void initDialogueUi()
    {
        var chestDescription_root_element = DialogueUiDocument.rootVisualElement;
        var text = chestDescription_root_element.Q<Label>("text");
        text.text = interactTextString[textIndex];
    }
    
    void changeDialogue()
    {
        textIndex++;
        print(textIndex);
        if (textIndex < interactTextString.Count)
        {
            var chestDescription_root_element = DialogueUiDocument.rootVisualElement;
            var text = chestDescription_root_element.Q<Label>("text");
            text.text = interactTextString[textIndex];
        }
        else
        {
            textIndex = 0;
            Time.timeScale = 1;
            dialogueUI.SetActive(false);
            inDialogue = false;
            DialogueBoxScript.OnDialogueNext -= changeDialogue;
        }
    }
}
