using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private TMP_Dropdown AIType;
    [SerializeField] private Image img;
    
    public static UIManager _instance;

    public HeroBlackboard heroBlackboard;
    private DungeonManager dungeonManager;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        dungeonManager = FindObjectOfType<DungeonManager>();
        
        StartCoroutine(AlphaLerp());

        //DontDestroyOnLoad(gameObject);
    }

    private IEnumerator AlphaLerp()
    {
        var alpha = img.color.a;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            img.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        yield break;
    }

    private void Start()
    {
        AIType.options.Clear();
        foreach (var personality in Enum.GetValues(typeof(Personnalities)))
        {
            AIType.options.Add(new TMP_Dropdown.OptionData(personality.ToString()));
        }
    }

    private void Update()
    {
        AIType.onValueChanged.AddListener(delegate { ChangeAIType(); });
    }

    private void ChangeAIType()
    {
        heroBlackboard.personality = (Personnalities) AIType.value;
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void NextLevel()
    {
        dungeonManager.LoadNextLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
