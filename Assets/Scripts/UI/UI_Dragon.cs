using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dragon : MonoBehaviour
{
    public static event Action OnDragonDeathEvent;
    public static event Action OnDragonTakeDamageEvent;
    
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject singleHeart;
    [SerializeField] private Image singleHeartImg;
    [SerializeField] private UI_Heart heartPrefab;
    [SerializeField] private Image dragonImage;
    [SerializeField] private Transform dragonCard;
    [SerializeField] private Image dragImg;
    [SerializeField] private TMP_Text healthText;
    List<UI_Heart> hearts = new();
    public float shakeDuration = 0.5f;

    public static int currentHealth = 10;
    public static int maxHealth = 10;
    public int damage = 3;
    
    [SerializeField] private Animator fireBallPrefab;
    [SerializeField] private AnimationClip fireBallAnim;

    private IEnumerator TakeDamageFX(Hero hero)
    {
        SoundManagerIngame.Instance.PlayDialogueSFX("DragonBreath");
        SoundManagerIngame.Instance.PlayDialogueSFX("DragonDamaged");
        yield return new WaitForSeconds(0.3f);
        var dragonImageColor = dragonImage.color;
        dragonImageColor.a = 0f;
        dragonImage.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        dragImg.color = Color.red;
        dragonCard.transform.DOShakePosition(shakeDuration, 0.4f, 10, 90, false, true);
        currentHealth -= 1;
        DrawHearts();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDragonDeathEvent?.Invoke();
            SoundManagerIngame.Instance.PlaySound(EmoteType.DragonDeath);
            yield break;
        }
        yield return new WaitForSeconds(shakeDuration);
        Animator fireBall = Instantiate(fireBallPrefab, dragonCard);
        fireBall.Play(fireBallAnim.name);
        yield return new WaitForSeconds(fireBallAnim.length);
        Camera.main.transform.DOShakePosition(shakeDuration, 0.4f, 10, 90, false, true);
        Destroy(fireBall.gameObject);
        dragImg.color = Color.white;
        hero.TakeDamage(damage, AttackType.Physical);
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
        
        for (int i = 0; i < maxHealth - currentHealth; i++)
        {
            CreateEmptyHeart();
        }
        
        //draw hearts
        if (currentHealth > maxHealth)
        {
            maxHealth = currentHealth;
        }
        singleHeart.transform.DOScale(0.0065f, 0.1f).OnComplete(() => { singleHeart.transform.DOScale(0.006f, 0.1f); });
        healthText.text = currentHealth.ToString();
        if (singleHeartImg)
            singleHeartImg.fillAmount = (float)currentHealth / maxHealth;
        
    }

    public void CreateFullHeart()
    {
        UI_Heart heart = Instantiate(heartPrefab, healthBar.transform);
        heart.transform.rotation = Quaternion.Euler(0, 0, 45);
        heart.transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBounce);
        heart.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            heart.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        });
        heart.SetHeartState(HeartState.Full);
        hearts.Add(heart);
    }

    public void CreateEmptyHeart()
    {
        UI_Heart heart = Instantiate(heartPrefab, healthBar.transform);
        heart.SetHeartState(HeartState.Empty);
        hearts.Add(heart);
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
    
    IEnumerator AttackByHero(float delay, Hero hero)
    {
        yield return new WaitForSeconds(delay);
        TakeDamage(hero.info.So.AttackPoint, hero);
        OnDragonTakeDamageEvent?.Invoke();
        DrawHearts();
    }
    
    public void CheckDragonHP(Hero hero)
    {
        StartCoroutine(AttackByHero(0.5f, hero));
    }
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        DrawHearts();
    }

    private void Start()
    {
        DrawHearts();
        Hero.OnMovedOnEmptyCardEvent += CheckDragonHP;
    }

    private void OnDisable()
    {
        Hero.OnMovedOnEmptyCardEvent -= CheckDragonHP;
    }
}