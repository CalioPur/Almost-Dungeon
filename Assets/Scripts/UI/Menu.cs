using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private bool  resetLevelIndexOnPlay = true;
    
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
        levelSelection.SetActive(true);
    }
}
