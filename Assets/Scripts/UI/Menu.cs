using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private Image fadeImage;
    [SerializeField] private List<TMP_Text> tryText;
    [SerializeField] private List<TMP_Text> highText;

    void Start()
    {
        LoadArcadeStats();
        SaveSystem.LoadSave();
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);

        if (SaveSystem.currentDungeon == -1)
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.onClick.AddListener(Continue);
        }
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void Play()
    {
        //levelSelection.SetActive(true);
        StartCoroutine(SwitchToLevelSelection(PlayerPrefs.HasKey("FinishedTutorial")));
    }

    private void Continue()
    {
        //levelSelection.SetActive(true);
        StartCoroutine(LoadLevelDirectly());
    }

    private IEnumerator LoadLevelDirectly()
    {
        videoPlayer.playbackSpeed = 1;
        videoPlayer.Play();
        fadeImage.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        DungeonManager._instance.SetSelectedBiomeAndLevelFromSave(SaveSystem.currentLevel, SaveSystem.currentDungeon);
    }

    IEnumerator SwitchToLevelSelection(bool hasFinishedTutorial)
    {
        videoPlayer.playbackSpeed = 1;
        videoPlayer.Play();
        fadeImage.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        
        if (hasFinishedTutorial)
        {
            SceneManager.LoadScene("LevelSelection");
        }
        else
        {
            DungeonManager._instance.SetSelectedBiome(0);
        }
        // PlayerPrefs.SetInt("FinishedTutorial", 1);
        // SceneManager.LoadScene("LevelSelection");
    }
    
    private void LoadArcadeStats()
    {
        for(int i = 0; i<3; i++)
        {
            tryText[i].text ="Try : " + PlayerPrefs.GetInt("MachTry" + (i+1), 0);
            highText[i].text = "High Score : " + PlayerPrefs.GetInt("MachHigh" + (i + 1), 0);
        }
    }
}