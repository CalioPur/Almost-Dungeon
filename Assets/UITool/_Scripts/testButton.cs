using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Slider = UnityEngine.UIElements.Slider;
using Dropdown = UnityEngine.UIElements.DropdownField;

public class testButton : MonoBehaviour
{
    [SerializeField] private UIDocument menuUiDocument;
    [SerializeField] private UIDocument settingsUiDocument;
    
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup SFXMixer;
    
    [SerializeField] private AudioSource sfxAudioSource;
    
    [SerializeField] private List<ButtonsMenu> buttonsMenus;
    
    private VisualElement menu_root_element;
    private VisualElement settings_root_element;
    
    [Serializable]
    public struct ButtonsMenu
    {
    
        [SerializeField] public TMP_Text text;
        [SerializeField] public List<string> labels;
    }
    
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        SFXMixer.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0));
        musicMixer.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0));
        XMLReader.currentLanguage = PlayerPrefs.GetInt("langue", 0);
        SetupMainMenuInteractions();

        UpdateLanguageButtonMenu();
    }

    private void UpdateLanguageButtonMenu()
    {
        foreach (var btn in buttonsMenus)
        {
            btn.text.text = btn.labels[PlayerPrefs.GetInt("langue", 0)];
        }
    }

    void SetupMainMenuInteractions()
    {
        if(!menuUiDocument) return;
        menu_root_element = menuUiDocument.rootVisualElement;
        var startGameButton = menu_root_element.Q<Button>("StartGame");
        //change label of the button
        startGameButton.text = XMLReader.languages[XMLReader.currentLanguage]["jouer"];
        startGameButton.RegisterCallback<ClickEvent>(StartGame);
        
        var Settings = menu_root_element.Q<Button>("Settings");
        Settings.text = XMLReader.languages[XMLReader.currentLanguage]["options"];
        Settings.RegisterCallback<ClickEvent>(OpenSettings);
        
        var Exit = menu_root_element.Q<Button>("Quit");
        Exit.text = XMLReader.languages[XMLReader.currentLanguage]["quitter"];
        Exit.RegisterCallback<ClickEvent>(ExitGame);
    }
    void SetupSettingsInteractions()
    {
        
        settings_root_element = settingsUiDocument.rootVisualElement;
        var Back = settings_root_element.Q<Button>("CloseSettings");
        Back.RegisterCallback<ClickEvent>(CloseSettings);
        
        var musicVolume = settings_root_element.Q<Slider>("MusicVolume");
        musicVolume.label = XMLReader.languages[XMLReader.currentLanguage]["musique"];
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0);
        musicVolume.RegisterCallback<ChangeEvent<float>>(ChangeMusicVolume);
        
        var soundVolume = settings_root_element.Q<Slider>("SoundVolume");
        soundVolume.value = PlayerPrefs.GetFloat("SFXVolume", 0);
        soundVolume.label = XMLReader.languages[XMLReader.currentLanguage]["son"];
        soundVolume.RegisterCallback<ChangeEvent<float>>(ChangeSoundVolume);
        soundVolume.RegisterCallback<ClickEvent>(PlaySound);
        
        var screenModeDropdown = settings_root_element.Q<Dropdown>("screenMode");
        screenModeDropdown.label = XMLReader.languages[XMLReader.currentLanguage]["ecran"];
        screenModeDropdown.choices = new List<string>(){XMLReader.languages[XMLReader.currentLanguage]["pleinEcran"], 
            XMLReader.languages[XMLReader.currentLanguage]["fenetré"], 
            XMLReader.languages[XMLReader.currentLanguage]["sansBordure"]};
        screenModeDropdown.index = PlayerPrefs.GetInt("ScreenMode", 0);
        screenModeDropdown.RegisterCallback<ChangeEvent<string>>(ChangeScreenMode);
        
        var languageDropdown = settings_root_element.Q<Dropdown>("Language");
        languageDropdown.label = XMLReader.languages[XMLReader.currentLanguage]["langue"];
        languageDropdown.index = PlayerPrefs.GetInt("langue", 0);
        languageDropdown.RegisterCallback<ChangeEvent<string>>(ChangeLanguage);
        
        // var dragnDrop = settings_root_element.Q<Toggle>("dragndroptoggle");
        // dragnDrop.value = PlayerPrefs.GetInt("DragNDrop", 0) == 1;
        // dragnDrop.RegisterCallback(new EventCallback<ChangeEvent<bool>>(evt =>
        // {
        //     if (evt.newValue)
        //     {
        //         Debug.Log("DragNDrop");
        //         PlayerPrefs.SetInt("DragNDrop true", 1);
        //     }
        //     else
        //     {
        //         Debug.Log("DragNDrop false");
        //         PlayerPrefs.SetInt("DragNDrop", 0);
        //     }
        //     SaveSystem.SaveAll();
        // }));
        // search for the toggle dragndroptoggle
        var dragnDrop = settings_root_element.Q<Toggle>("dragndroptoggle");
        dragnDrop.value = PlayerPrefs.GetInt("DragNDrop", 0) == 1;
        dragnDrop.RegisterCallback<ChangeEvent<bool>>(ChangeDragNDrop);
        
    }

    private void ChangeDragNDrop(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Debug.Log("DragNDrop");
            PlayerPrefs.SetInt("DragNDrop", 1);
        }
        else
        {
            Debug.Log("DragNDrop false");
            PlayerPrefs.SetInt("DragNDrop", 0);
        }
        SaveSystem.SaveAll();
    }

    private void ChangeLanguage(ChangeEvent<string> evt)
    {
        if (evt.newValue == "Français")
        {
            XMLReader.currentLanguage = 1;
            PlayerPrefs.SetInt("langue", 1);
            print("LANGUE FR");
        }
        else if (evt.newValue == "English")
        {
            XMLReader.currentLanguage = 0;
            PlayerPrefs.SetInt("langue", 0);
            print("LANGUE EN");
        }
        else if (evt.newValue == "Miaou")
        {
            XMLReader.currentLanguage = 2;
            PlayerPrefs.SetInt("langue", 2);
        }
        SetupMainMenuInteractions();
        SetupSettingsInteractions();
        UpdateLanguageButtonMenu();
    }

    private void ChangeScreenMode(ChangeEvent<string> evt)
    {
        print("ScreenMode changed");
        if (evt.newValue == XMLReader.languages[XMLReader.currentLanguage]["pleinEcran"])
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetInt("ScreenMode", 0);
            print("Fullscreen");
        }
        else if (evt.newValue == XMLReader.languages[XMLReader.currentLanguage]["fenetré"])
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("ScreenMode", 1);
            print("Windowed");
        }
        else if (evt.newValue == XMLReader.languages[XMLReader.currentLanguage]["sansBordure"])
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            PlayerPrefs.SetInt("ScreenMode", 2);
            print("Borderless");
        }
    }

    private void PlaySound(ClickEvent evt)
    {
        sfxAudioSource.Play();
        
    }

    private void ChangeSoundVolume(ChangeEvent<float> evt)
    {
        SFXMixer.audioMixer.SetFloat("SFXVolume", evt.newValue);
        PlayerPrefs.SetFloat("SFXVolume", evt.newValue);
    }

    private void ChangeMusicVolume(ChangeEvent<float> evt)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", evt.newValue);
        PlayerPrefs.SetFloat("MusicVolume", evt.newValue);
    }

    public void CloseSettings(ClickEvent evt)
    {
        settings.SetActive(false);
    }

    void StartGame(ClickEvent ev)
    {
        SceneManager.LoadScene(1);
    }
    
    void OpenSettings(ClickEvent ev)
    {
        settings.SetActive(true);
        SetupSettingsInteractions();
    }
    
    void ExitGame(ClickEvent ev)
    {
        Application.Quit();
    }
    
    public void OpenSettings()
    {
        settings.SetActive(true);
        SetupSettingsInteractions();
    }
    
    public void CloseSettings()
    {
        settings.SetActive(false);
    }
}
