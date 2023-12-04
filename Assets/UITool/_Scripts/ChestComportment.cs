using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestComportment : MonoBehaviour
{
    bool isChestOpen = false;
    bool isPlayerNear = false;

    [SerializeField] Sprite openChestSprite;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject interactText;
    [SerializeField] UIDocument interactTextUIDocument;
    [SerializeField] GameObject chestDescriptionUI;
    List<string> interactTextString;
    [SerializeField] private string identifier;
    
    [SerializeField] UIDocument chestDescriptionUiDocument;
    
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
        if (other.gameObject.CompareTag("Player") && !isChestOpen)
        {
            isPlayerNear = true;
            interactText.SetActive(true);
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
    
    //open chest when player press E
    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isChestOpen)
        {
            spriteRenderer.sprite = openChestSprite;
            isChestOpen = true;
            interactText.SetActive(false);
            chestDescriptionUI.SetActive(true);
            initChestDescriptionUi();
            DialogueBoxScript.OnDialogueNext += changeChestDescription;
        }
    }
    
    void changeChestDescription()
    {
        textIndex++;
        if (textIndex < interactTextString.Count)
        {
            var chestDescription_root_element = chestDescriptionUiDocument.rootVisualElement;
            var text = chestDescription_root_element.Q<Label>("text");
            text.text = interactTextString[textIndex];
        }
        else
        {
            chestDescriptionUI.SetActive(false);
            DialogueBoxScript.OnDialogueNext -= changeChestDescription;
        }
    }
    
    void initChestDescriptionUi()
    {
        var chestDescription_root_element = chestDescriptionUiDocument.rootVisualElement;
        var text = chestDescription_root_element.Q<Label>("text");
        text.text = interactTextString[0];
    }
}
