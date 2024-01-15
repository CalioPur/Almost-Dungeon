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
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private Image fadeImage;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        playButton.onClick.AddListener(Play);
        quitButton.onClick.AddListener(Quit);
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
    
    IEnumerator SwitchToLevelSelection()
    {
        
        videoPlayer.playbackSpeed = 1;
        videoPlayer.Play();
        fadeImage.DOFade(1, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LevelSelection");
    }
}
