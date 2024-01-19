using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseInGameScript : MonoBehaviour
{
    
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button resumeButton;
    [SerializeField] Toggle gameModeToggle;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    
    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioMixerGroup SFXMixer;

    private void Awake()
    {
        resumeButton.onClick.AddListener(Resume);
        gameModeToggle.onValueChanged.AddListener(GameModeToggle);
        musicSlider.onValueChanged.AddListener(MusicSlider);
        soundSlider.onValueChanged.AddListener(SoundSlider);
    }

    void Update()
    {
        //si la touche echapest appuyée
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }
    
    private void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    
    private void GameModeToggle(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("DragNDrop", 1);
        }
        else
        {
            PlayerPrefs.SetInt("DragNDrop", 0);
        }
    }
    
    private void MusicSlider(float value)
    {
        
        musicMixer.audioMixer.SetFloat("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume",value);
    }
    
    private void SoundSlider(float value)
    {
        SFXMixer.audioMixer.SetFloat("SFXVolume", value);
        PlayerPrefs.SetFloat("SFXVolume",value);
    }
    
}
