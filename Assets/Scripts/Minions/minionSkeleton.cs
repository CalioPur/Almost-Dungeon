using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class minionSkeleton : MinionData
{
    [HideInInspector] public bool isDigger;
    [HideInInspector] public bool isReadyToUndig;
    [SerializeField] private Transform model3DTr;
    [SerializeField] private Transform FXDigTr;

    protected override void Init()
    {
        base.Init();
        var spriteColor = sprite.color;
        sprite.color = spriteColor;
        isDigger = true;
        isReadyToUndig = false;
    }

    protected override void OnDead()
    {
        model3DTr.rotation = Quaternion.Euler(180 + transform.rotation.eulerAngles.x, 0, 0);
        minionInstance.CurrentHealthPoint = minionInstance.So.health;
        mapManager.GetWorldPosFromTilePos(new Vector2Int(indexX, indexY), out Vector3 worldPos);
        mapManager.RemoveEnemyOnTile(
            new Vector2Int(indexX, indexY), this, worldPos);
        isDigger = true;
        isReadyToUndig = false;
        model3DTr.DOMoveY(-0.3f, 0.1f).onComplete += () =>
        {
            FXDigTr.gameObject.SetActive(true);
            model3DTr.gameObject.SetActive(false);
        };
        
    }

    public void FinishToDig()
    {
        int index = 0;
        mapManager.AddMinionOnTile(
            new Vector2Int(indexX, indexY), this, ref index);
        model3DTr.DORotate(transform.rotation.eulerAngles, 0.1f);
        FXDigTr.gameObject.SetActive(false);
        model3DTr.gameObject.SetActive(true);
        model3DTr.DOMoveY(1, 0.1f).SetEase(Ease.OutElastic).onComplete += () =>
        {
            model3DTr.DOMoveY(0, 0.1f);
        };
    }
}