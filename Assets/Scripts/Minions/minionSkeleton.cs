using System;
using System.Collections.Generic;
using DG.Tweening;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.UI;

public class minionSkeleton : MinionData
{
    [HideInInspector] public bool isDigger;
    [HideInInspector] public bool isReadyToUndig;
    [SerializeField] private Transform model3DTr;
    [SerializeField] private Transform FXDigTr;
    private int resurectCount = 0;
    public static event Action<int> OnResurectEvent;
    
    private static List<MinionData> minions = new ();
    
    protected override void Init()
    {
        base.Init();
        var spriteColor = sprite.color;
        sprite.color = spriteColor;
        isDigger = true;
        isReadyToUndig = false;
        minions.Add(this);
    }

    protected override void OnDead()
    {
        model3DTr.rotation = Quaternion.Euler(180 + transform.rotation.eulerAngles.x, model3DTr.rotation.y,
            model3DTr.rotation.z);
        minionInstance.CurrentHealthPoint = minionInstance.So.health;
        mapManager.GetWorldPosFromTilePos(new Vector2Int(indexX, indexY), out Vector3 worldPos);
        mapManager.RemoveEnemyOnTile(
            new Vector2Int(indexX, indexY), this, worldPos);
        
        foreach (var skeleton in minions)
        {
            if (skeleton == this) continue;
            if (skeleton.indexX == indexX && skeleton.indexY == indexY)
            {
                Destroy(gameObject);
                return;
            }
        }
        
        isDigger = true;
        isReadyToUndig = false;
        model3DTr.DOMoveY(-0.9f, 0.1f).onComplete += () =>
        {
            FXDigTr.gameObject.SetActive(true);
            model3DTr.gameObject.SetActive(false);
        };
        
    }

    public void FinishToDig()
    {
        mapManager.AddMinionOnTile(
            new Vector2Int(indexX, indexY), this);
        //emodel3DTr.DORotate(transform.rotation.eulerAngles, 0.1f);
        FXDigTr.gameObject.SetActive(false);
        model3DTr.gameObject.SetActive(true);
        model3DTr.DOMoveY(1, 0.1f).SetEase(Ease.OutElastic).onComplete += () =>
        {
            model3DTr.DOMoveY(0.1f, 0.1f);
        };
        resurectCount++;
        OnResurectEvent?.Invoke(resurectCount);
    }
}