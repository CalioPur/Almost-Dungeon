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
    
    [SerializeField] private GameObject singleHeart;
    [SerializeField] private Image singleHeartImg;
    //[SerializeField] private UI_Heart heartPrefab;
    [SerializeField] private Image dragonImage;
    [SerializeField] private Transform dragonCard;
    [SerializeField] private TMP_Text healthText;
    List<UI_Heart> hearts = new();
    public float shakeDuration = 1.0f;

    public static int currentHealth = 10;
    public static int maxHealth = 10;
    public int damage = 3;
    
    // [SerializeField] private Animator fireBallPrefab;
    // [SerializeField] private AnimationClip fireBallAnim;
    [SerializeField] private ChangeMatDragon dragonPawn;
    
    private ChangeMatDragon dragonPawnMat = null;

    private IEnumerator TakeDamageFX(Hero hero, float delay)
    {
        if (hero.info.CurrentHealthPoint - damage <= 0) TickManager.Instance.OnEndGame();
        SoundManagerIngame.Instance.PlayDialogueSFX("DragonBreath");
        SoundManagerIngame.Instance.PlayDialogueSFX("DragonDamaged");
        yield return new WaitForSeconds(0.3f * TickManager.Instance.calculateBPM());
        var dragonImageColor = dragonImage.color;
        dragonImageColor.a = 0f;
        dragonImage.color = Color.red;
        dragonCard.transform.DOShakePosition(shakeDuration, 0.4f, 10, 90, false, true);
        currentHealth -= 1;
        DrawHearts();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameManager.Instance.EndOfGame = true;
            OnDragonDeathEvent?.Invoke();
            SoundManagerIngame.Instance.PlaySound(EmoteType.DragonDeath);
            yield break;
        }
        yield return new WaitForSeconds(shakeDuration);
        dragonPawnMat.RenderTransparent();
        
        // Animator fireBall = Instantiate(fireBallPrefab, dragonCard);
        // fireBall.Play(fireBallAnim.name);
        // Destroy(fireBall.gameObject, fireBallAnim.length);
        yield return new WaitForSeconds(delay);
        
        dragonPawnMat.DOKill();
        dragonPawnMat.gameObject.SetActive(false);
        Camera.main.transform.DOShakePosition(shakeDuration, 0.4f, 10, 90, false, true);
        
        hero.TakeDamage(damage, AttackType.Physical);
        
        dragonImage.color = Color.white;
        dragonImageColor.a = 1f;
    }

    #region Health

    private void DrawHearts()
    {
        DestroyAllHearts();
        int a = 0;
        
        //draw hearts
        if (currentHealth > maxHealth)
        {
            maxHealth = currentHealth;
        }
        singleHeart.transform.DOScale(0.0065f, 0.1f).OnComplete(() => { singleHeart.transform.DOScale(0.006f, 0.1f); });
        healthText.text = currentHealth.ToString();
        
        //XImage img = singleHeart.transform.GetChild(0).GetComponent<Image>();
        if (singleHeartImg)
            singleHeartImg.fillAmount = (float)currentHealth / maxHealth;
        
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
    
    IEnumerator AttackByHero(float delay, Hero hero, DirectionToMove attackDirection)
    {
        if (!dragonPawnMat)
            dragonPawnMat = Instantiate(dragonPawn);
        dragonPawnMat.gameObject.SetActive(true);
        dragonPawnMat.transform.position = GameManager.Instance.AttackPoint.position;
        dragonPawnMat.RenderTransparent();
        Vector3 rotation = Vector3.zero;
        switch (attackDirection)
        {
            case DirectionToMove.Down:
                rotation = new Vector3(0, 0, 0);
                break;
            case DirectionToMove.Up:
                rotation = new Vector3(0, 180, 0);
                break;
            case DirectionToMove.Right:
                rotation = new Vector3(0, 270, 0);
                break;
            case DirectionToMove.Left:
                rotation = new Vector3(0, 90, 0);
                break;
        }
        dragonPawnMat.transform.rotation = Quaternion.Euler(rotation);
        dragonPawnMat.transform.DOShakePosition(10, 0.1f, 5, 90, false, true);
        yield return new WaitForSeconds(delay);
        dragonPawnMat.RenderOpaque();
        StartCoroutine(TakeDamageFX(hero, delay));
        OnDragonTakeDamageEvent?.Invoke();
        DrawHearts();
    }
    
    public void CheckDragonHP(Hero hero, DirectionToMove attackDirection)
    {
        if (currentHealth <= 0) return;
        StartCoroutine(AttackByHero(TickManager.Instance.calculateBPM() * 0.5f, hero, attackDirection));
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
        TickManager.Instance.OnHeroTick += HeartBeat;
        TickManager.Instance.OnMinionTick += HeartBeat;
    }

    private void OnDisable()
    {
        Hero.OnMovedOnEmptyCardEvent -= CheckDragonHP;
        TickManager.Instance.OnHeroTick -= HeartBeat;
        TickManager.Instance.OnMinionTick -= HeartBeat;
    }

    private void HeartBeat()
    {
        singleHeart.transform.DOScale(0.0065f, 0.1f).OnComplete(() => { singleHeart.transform.DOScale(0.006f, 0.1f); });
    }
}