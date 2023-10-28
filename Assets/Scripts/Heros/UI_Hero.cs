using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hero : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject heartPrefab;
    List<UI_HeroHeart> hearts = new();


    [Header("Test Value to delete")] public float playerHealth = 6;
    public float currentHealth = 6;

    public void DrawHearts()
    {
        DestroyAllHearts();

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
    
    private void Start()
    {
        DrawHearts();
    }
    
    
    //test update will be deleted later
    private void Update()
    {
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
    }
}