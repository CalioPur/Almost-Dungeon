using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

using Dropdown = UnityEngine.UIElements.DropdownField;

public class UiManagerInGame : MonoBehaviour
{
    [SerializeField] private UIDocument pauseUiDocument;
    [SerializeField] private UIDocument settingsUiDocument;
    
    [SerializeField] public GameObject pause;
    [SerializeField] public GameObject settings;
    
    
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup SFXMixer;
    
    [SerializeField] private AudioSource sfxAudioSource;
    
    public static event Action OnLanguageChange; 

    private void Start()
    {
        SFXMixer.audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0));
        musicMixer.audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0));
    }

    // Update is called once per frame
    void Update()
    {
        //if the player press escape, the game will pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pause.SetActive(true);
        initPauseUi();
    }

    private void initPauseUi()
    {
        var pause_root_element = pauseUiDocument.rootVisualElement;
        
        var close = pause_root_element.Q<Button>("ClosePause");
        close.RegisterCallback<ClickEvent>(ResumeGame);
        
        var resume = pause_root_element.Q<Button>("Resume");
        resume.text = XMLReader.languages[XMLReader.currentLanguage]["reprendre"];
        resume.RegisterCallback<ClickEvent>(ResumeGame);

        var settingsButton = pause_root_element.Q<Button>("Settings");
        settingsButton.text = XMLReader.languages[XMLReader.currentLanguage]["options"];
        settingsButton.RegisterCallback<ClickEvent>(OpenSettings);

        var exit = pause_root_element.Q<Button>("MainMenu");
        exit.text = XMLReader.languages[XMLReader.currentLanguage]["menuPrincipal"];
        exit.RegisterCallback<ClickEvent>(GoToMenu);
    }

    private void GoToMenu(ClickEvent evt)
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    private void ResumeGame(ClickEvent evt)
    {
        Time.timeScale = 1;
        pause.SetActive(false);
    }
    
    private void OpenSettings(ClickEvent evt)
    {
        settings.SetActive(true);
        initSettingsUi();
    }

    private void initSettingsUi()
    {
        var settings_root_element = settingsUiDocument.rootVisualElement;
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
        OnLanguageChange?.Invoke();
        initPauseUi();
        initSettingsUi();
    }
    private void ChangeScreenMode(ChangeEvent<string> evt)
    {
        
        if (evt.newValue == XMLReader.languages[XMLReader.currentLanguage]["pleinEcran"])
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetInt("ScreenMode", 0);
            
        }
        else if (evt.newValue == XMLReader.languages[XMLReader.currentLanguage]["fenetré"])
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("ScreenMode", 1);
            
        }
        else if (evt.newValue == XMLReader.languages[XMLReader.currentLanguage]["sansBordure"])
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            PlayerPrefs.SetInt("ScreenMode", 2);
            
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
    
}
