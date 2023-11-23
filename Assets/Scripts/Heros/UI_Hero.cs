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
    [SerializeField] private GameObject heartPrefab;

    [SerializeField] private GameObject ItemBar;
    [SerializeField] private GameObject itemsPrefab;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text endGameText;
    [SerializeField] private string[] itemNamesChoices;

    [SerializeField] private TMP_Text heroPersonality;
    [SerializeField] private TMP_Text heroLevel;
    [SerializeField] private TMP_Text heroName;

    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private Image heroImage;

    [SerializeField] private int itemSlotsCount = 3;

    private List<UI_Heart> hearts = new();
    private List<UI_HeroItem> items = new();

    #region Health



    private void DrawHearts(int _currentHealth, bool newHeart)
    {
        DestroyAllHearts();

        if (_currentHealth <= 0)
        {
            OnEndGameEvent?.Invoke(true);
        }

        float maxHealthRemainder = _currentHealth % 2;
        int maxHealth = (int)((_currentHealth / 2) + maxHealthRemainder);
        for (int i = 0; i < maxHealth; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartState = (int)Mathf.Clamp(_currentHealth - i * 2, 0, 2);
            hearts[i].SetHeartState((HeartState)heartState);
        }
        if (newHeart)
            StartCoroutine(TakeDamageFX());
    }

    public void CreateFullHeart()
    {
        GameObject heart = Instantiate(heartPrefab, healthBar.transform);
        UI_Heart heartScript = heart.GetComponent<UI_Heart>();
        heartScript.SetHeartState(HeartState.Full);
        hearts.Add(heartScript);
    }

    public void CreateHalfHeart()
    {
        GameObject heart = Instantiate(heartPrefab, healthBar.transform);
        UI_Heart heartScript = heart.GetComponent<UI_Heart>();
        heartScript.SetHeartState(HeartState.Half);
        hearts.Add(heartScript);
    }

    public void CreateEmptyHeart()
    {
        GameObject heart = Instantiate(heartPrefab, healthBar.transform);
        UI_Heart heartScript = heart.GetComponent<UI_Heart>();
        heartScript.SetHeartState(HeartState.Empty);
        hearts.Add(heartScript);
    }

    public void DestroyAllHearts()
    {
        foreach (UI_Heart heart in hearts)
        {
            Destroy(heart.gameObject);
        }

        hearts.Clear();
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
        heroLevel.text = heroData.heroLevel.ToString();
        heroPersonality.text = heroData.heroPersonality;
    }

    public IEnumerator TakeDamageFX()
    {


        heroImage.color = Color.red;
        heroImage.transform.DOShakePosition(shakeDuration, 10, 10, 90, false, true);
        yield return new WaitForSeconds(shakeDuration);
        heroImage.color = Color.white;
    }

    void EndGame(bool win)
    {
        Time.timeScale = 0;
        endGameText.text = win ? "You Win !" : "You Lose !";
        endGamePanel.SetActive(true);

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
        UI_Dragon.OnDragonDeathEvent += LoseGame;
    }

    private void OnDisable()
    {
        Hero.OnPopUpEvent -= SetupValues;
        UI_Dragon.OnDragonDeathEvent -= LoseGame;
        Hero.OnTakeDamageEvent -= DrawHearts;
        OnEndGameEvent -= EndGame;
    }

    private void Start()
    {
        if (ItemBar.activeSelf) DrawItems();
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