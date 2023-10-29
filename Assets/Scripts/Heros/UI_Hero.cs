using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class UI_Hero : MonoBehaviour
{
    public static event Action<bool> OnEndGameEvent;
    
    public GameObject healthBar;
    public GameObject heartPrefab;
    List<UI_HeroHeart> hearts = new();

    public GameObject ItemBar;
    public GameObject itemsPrefab;
    public GameObject endGamePanel;
    List<UI_HeroItem> items = new();
    public string[] itemNamesChoices;
    
    public TMP_Text timeLeftBeforeNextMove;
    public TMP_Text heroPersonality;
    public TMP_Text heroLevel;
    public TMP_Text heroName;


    [Header("Test Value to delete")] public float playerHealth = 6;
    public float currentHealth = 6;

    public int itemSlotsCount = 3;
    private float timeLeft = 0f;

    #region Health
    
    

    public void DrawHearts()
    {
        DestroyAllHearts();

        if (currentHealth <= 0)
        {
            OnEndGameEvent?.Invoke(true);
        }
            
        
        float maxHealthRemainder = playerHealth % 2;
        int maxHealth = (int)((playerHealth / 2) + maxHealthRemainder);
        for (int i = 0; i < maxHealth; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartState = (int)Mathf.Clamp(currentHealth - i * 2, 0, 2);
            hearts[i].SetHeartState((HeartState)heartState);
        }
    }

    private void CreateFullHeart()
    {
        GameObject heart = Instantiate(heartPrefab, healthBar.transform);
        UI_HeroHeart heartScript = heart.GetComponent<UI_HeroHeart>();
        heartScript.SetHeartState(HeartState.Full);
        hearts.Add(heartScript);
    }

    private void CreateHalfHeart()
    {
        GameObject heart = Instantiate(heartPrefab, healthBar.transform);
        UI_HeroHeart heartScript = heart.GetComponent<UI_HeroHeart>();
        heartScript.SetHeartState(HeartState.Half);
        hearts.Add(heartScript);
    }

    public void CreateEmptyHeart()
    {
        GameObject heart = Instantiate(heartPrefab, healthBar.transform);
        UI_HeroHeart heartScript = heart.GetComponent<UI_HeroHeart>();
        heartScript.SetHeartState(HeartState.Empty);
        hearts.Add(heartScript);
    }

    public void DestroyAllHearts()
    {
        foreach (UI_HeroHeart heart in hearts)
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
    
    public void UpdateTimeLeftBeforeNextMove(float time)
    {
        timeLeftBeforeNextMove.text = time.ToString("0.0");
    }

    void EndGame(bool win)
    {
        
    }

    void LoseGame()
    {
    }
    

    private void Start()
    {
        DrawHearts();
        DrawItems();
        Hero.OnMovedOnEmptyCardEventEvent += () => EndGame(false);
        
        //test hero data
        HeroData heroData = new HeroData();
        heroData.heroName = "Hero's Name";
        heroData.heroLevel = 1;
        heroData.heroPersonality = "Hero's Personality";
        SetHeroData(heroData);
    }


    //test update will be deleted later
    private void Update()
    {
        //test health
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentHealth--;
            DrawHearts();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentHealth++;
            DrawHearts();
        }
        
        //test time
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timeLeft = 10f;
        }
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimeLeftBeforeNextMove(timeLeft);
        }
    }
}

//test class will be deleted later
public class HeroData
{
    public string heroName;
    public int heroLevel;
    public string heroPersonality;
}