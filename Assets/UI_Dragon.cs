using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dragon : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject heartPrefab;
    public Image dragonImage;
    List<UI_Heart> hearts = new();
    public float shakeDuration = 0.5f;
    
    public int currentHealth = 100;
    public int damage = 3;
    
    public static UI_Dragon Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        DrawHearts();
    }
    
    public IEnumerator TakeDamageFX(Hero hero)
    {
        
        
        dragonImage.color = Color.red;
        dragonImage.transform.DOShakePosition(shakeDuration, 10, 10, 90, false, true);
        yield return new WaitForSeconds(shakeDuration);
        dragonImage.color = Color.white;
        hero.TakeDamage(UI_Dragon.Instance.damage);
    }

    #region Health

    public void DrawHearts()
    {
        DestroyAllHearts();

        float maxHealthRemainder = currentHealth % 2;
        int maxHealth = (int)((currentHealth / 2) + maxHealthRemainder);
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

    public void TakeDamage(int damage, Hero hero)
    {
        currentHealth -= damage;
        StartCoroutine(TakeDamageFX(hero));
        DrawHearts();
    }
    
    // Additional methods and properties specific to the dragon can be added here.

    private void Start()
    {
        // Initialization logic specific to the dragon UI can be placed here.
    }
}