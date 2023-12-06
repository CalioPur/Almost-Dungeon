using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private VisualElement menu_root_element;
    private VisualElement settings_root_element;
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        SFXMixer.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0));
        musicMixer.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0));
        XMLReader.currentLanguage = PlayerPrefs.GetInt("Language", 0);
        SetupMainMenuInteractions();
        
        
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
        languageDropdown.index = PlayerPrefs.GetInt("Language", 0);
        languageDropdown.RegisterCallback<ChangeEvent<string>>(ChangeLanguage);
        
    }

    private void ChangeLanguage(ChangeEvent<string> evt)
    {
        if (evt.newValue == "Français")
        {
            XMLReader.currentLanguage = 1;
            PlayerPrefs.SetInt("Language", 1);
        }
        else if (evt.newValue == "English")
        {
            XMLReader.currentLanguage = 0;
            PlayerPrefs.SetInt("Language", 0);
        }
        else if (evt.newValue == "Miaou")
        {
            XMLReader.currentLanguage = 2;
            PlayerPrefs.SetInt("Language", 2);
        }
        SetupMainMenuInteractions();
        SetupSettingsInteractions();
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

    private void CloseSettings(ClickEvent evt)
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
}
