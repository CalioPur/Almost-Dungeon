using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    //[SerializeField] private TMP_Dropdown AIType;
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
        

        //DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(AlphaLerp());
    }

    private IEnumerator AlphaLerp()
    {
        var alpha = img.color.a;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            img.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        yield break;
    }
    
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
    public void NextLevel()
    {
        print("next level, maybe done twice, this is a bug");
        dungeonManager.LoadNextLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
