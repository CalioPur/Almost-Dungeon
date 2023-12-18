using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dragon : MonoBehaviour
{
    public static event Action OnDragonDeathEvent;
    
    public GameObject healthBar;
    public GameObject heartPrefab;
    public Image dragonImage;
    List<UI_Heart> hearts = new();
    public float shakeDuration = 0.5f;

    public static int currentHealth;
    public static int maxHealth = 0;
    public int damage = 3;

    private IEnumerator TakeDamageFX(Hero hero)
    {
        yield return new WaitForSeconds(0.5f);
        var dragonImageColor = dragonImage.color;
        dragonImageColor.a = 0f;
        dragonImage.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        dragonImage.transform.DOShakePosition(shakeDuration, 10, 10, 90, false, true);
        currentHealth -= 1;
        hero.TakeDamage(damage);
        DrawHearts();
        yield return new WaitForSeconds(shakeDuration);
        dragonImage.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        dragonImageColor.a = 1f;
    }

    #region Health

    private void DrawHearts()
    {
        DestroyAllHearts();
        int a = 0;
        //draw full hearts and empty hearts full heart = currentHealth empty heart = maxHealth - currentHealth
        for (int i = 0; i < currentHealth; i++)
        {
            CreateFullHeart();
            a++;
        }
        Debug.Log(a);
        for (int i = 0; i < maxHealth - currentHealth; i++)
        {
            CreateEmptyHeart();
        }
        
        //draw hearts
        
    }

    public void CreateFullHeart()
    {
        GameObject heart = Instantiate(heartPrefab, healthBar.transform);
        UI_Heart heartScript = heart.GetComponent<UI_Heart>();
        heartScript.SetHeartState(HeartState.Full);
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
        StartCoroutine(TakeDamageFX(hero));
    }
    
    public void CheckDragonHP(Hero hero)
    {
        TakeDamage(hero.info.So.AttackPoint, hero);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDragonDeathEvent?.Invoke();
            SoundManagerIngame.Instance.PlaySound(EmoteType.DragonDeath);
        }

        DrawHearts();
    }

    private void Start()
    {
        maxHealth = GameManager._instance.dragonHealthPoint;
        currentHealth = maxHealth;
        DrawHearts();
        Hero.OnMovedOnEmptyCardEvent += CheckDragonHP;
    }

    private void OnDisable()
    {
        Hero.OnMovedOnEmptyCardEvent -= CheckDragonHP;
    }
}