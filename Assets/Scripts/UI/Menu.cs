using System;
using System.Collections;
using DG.Tweening;
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

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        SaveSystem.LoadSave();
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);
        
        if (SaveSystem.currentDungeon == -1)
        {
            continueButton.interactable = false;
        }
        else
        {
            Debug.Log("Current dungeon: " + SaveSystem.currentDungeon + " Current level: " + SaveSystem.currentLevel);
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
        StartCoroutine(SwitchToLevelSelection());
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
        DungeonManager._instance.SetSelectedBiomeAndLevelFromSave(SaveSystem.currentLevel,SaveSystem.currentDungeon);
    }

    IEnumerator SwitchToLevelSelection()
    {
        
        videoPlayer.playbackSpeed = 1;
        videoPlayer.Play();
        fadeImage.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LevelSelection");
    }
}
