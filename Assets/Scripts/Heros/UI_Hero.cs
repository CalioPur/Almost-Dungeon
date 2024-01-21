using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hero : MonoBehaviour
{
    public static event Action<bool> OnEndGameEvent;

    [SerializeField] private GameObject healthBar;

    [SerializeField] private GameObject ItemBar;
    [SerializeField] private GameObject itemsPrefab;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text endGameText;
    [SerializeField] private Button endGameButton;
    [SerializeField] private string[] itemNamesChoices;

    public TMP_Text heroPersonality;
    public TMP_Text heroName;

    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private Image heroImage;

    [SerializeField] private int itemSlotsCount = 3;

    private List<UI_Heart> hearts = new();
    private List<UI_HeroItem> items = new();
    private int maxHealth = 0;

    #region Health
    

    private void DrawHearts(int _currentHealth, bool newHeart)
    {
        if (_currentHealth > maxHealth)
        {
            maxHealth = _currentHealth;
        }
        if (_currentHealth <= 0)
        {
            OnEndGameEvent?.Invoke(true);
        }
        healthBar.transform.DOScale(0.0065f, 0.1f).OnComplete(() => { healthBar.transform.DOScale(0.006f, 0.1f); });
        healthBar.GetComponentInChildren<TMP_Text>().text = _currentHealth.ToString();
        healthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)_currentHealth / maxHealth;
        
    }

    #endregion

    #region Items

    private void DrawItems()
    {
        DestroyAllItems();
        for (int i = 0; i < itemSlotsCount; i++)
        {
            CreateEmptyItem();
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetItem(itemNamesChoices[i]);
        }
    }

    private void CreateEmptyItem()
    {
        GameObject item = Instantiate(itemsPrefab, ItemBar.transform);
        UI_HeroItem itemScript = item.GetComponent<UI_HeroItem>();
        items.Add(itemScript);
    }

    private void DestroyAllItems()
    {
        foreach (UI_HeroItem item in items)
        {
            Destroy(item.gameObject);
        }

        items.Clear();
    }

    #endregion

    public void SetHeroData(HeroData heroData)
    {
        heroName.text = heroData.heroName;
        heroPersonality.text = heroData.heroPersonality;
    }

    public IEnumerator TakeDamageFX()
    {
        heroImage.color = Color.red;
        Vector3 originalPos = heroImage.transform.parent.transform.position;
        heroImage.transform.parent.transform.position += new Vector3(0, 0.2f, 0);
        heroImage.transform.parent.transform.DOShakePosition(shakeDuration, 0.4f, 10, 90, false, true);
        yield return new WaitForSeconds(shakeDuration + 0.05f);
        heroImage.transform.parent.transform.position = originalPos;
        heroImage.color = Color.white;
    }
    
    private void TakeDamage(int _, bool __)
    {
        StartCoroutine(TakeDamageFX());
    }

    void EndGame(bool win)
    {
        if (win)
        {
            endGameText.text = "Vous avez roti "+ heroName.text + " !";
            endGameText.color = Color.green;
            if (DungeonManager._instance.currentLevel < 6)
            {
                endGameButton.GetComponentInChildren<TMP_Text>().text = "Passer au niveau " + (DungeonManager._instance.currentLevel+2);
            }
            else
            {
                endGameButton.GetComponentInChildren<TMP_Text>().text = "Main Menu";
            }
            endGameButton.onClick.RemoveAllListeners();
            endGameButton.onClick.AddListener((() => { UIManager._instance.NextLevel(); }));
        }
        else
        {
            endGameText.text = heroName.text + " vous a vaincu !";
            endGameText.color = Color.red;
            endGameButton.GetComponentInChildren<TMP_Text>().text = "Main Menu";
            DungeonManager._instance.ResetLevelIndex();
            endGameButton.onClick.RemoveAllListeners();
            //make button go back to scene 0
            endGameButton.onClick.AddListener(() => { UnityEngine.SceneManagement.SceneManager.LoadScene(0); });
        }
        
        //endGamePanel.SetActive(true);
        StartCoroutine(EndGameFX());
    }
    
    private IEnumerator EndGameFX()
    {
        yield return new WaitForSeconds(2.5f);
        endGamePanel.SetActive(true);
        Time.timeScale = 0;
        yield return null;
    }
    
    void LoseGame()
    {
        OnEndGameEvent?.Invoke(false);
    }

    private void SetupValues(int currentHealth)
    {
        DrawHearts(currentHealth, false);
    }


    private void Awake()
    {
        Hero.OnPopUpEvent += SetupValues;
        Hero.OnTakeDamageEvent += TakeDamage;
        UI_Dragon.OnDragonDeathEvent += LoseGame;
    }



    private void OnDisable()
    {
        Hero.OnPopUpEvent -= SetupValues;
        UI_Dragon.OnDragonDeathEvent -= LoseGame;
        Hero.OnTakeDamageEvent -= DrawHearts;
        OnEndGameEvent -= EndGame;
        Hero.OnTakeDamageEvent -= TakeDamage;
        OnEndGameEvent = null;
    }

    private void Start()
    {
        Hero.OnTakeDamageEvent += TakeDamage;
        Hero.OnTakeDamageEvent += DrawHearts;
        OnEndGameEvent += EndGame;
    }
}

public class HeroData
{
    public string heroName;
    public int heroLevel;
    public string heroPersonality;
}