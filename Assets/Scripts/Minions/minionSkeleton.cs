using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class minionSkeleton : MinionData
{
    [HideInInspector] public bool isDigger;
    [HideInInspector] public bool isReadyToUndig;
    [SerializeField] private Transform model3DTr;

    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }

    protected override void Init()
    {
        base.Init();
        Hero.OnGivePosBackEvent += GetHeroPos;
        var spriteColor = sprite.color;
        sprite.color = spriteColor;
        isDigger = true;
        isReadyToUndig = false;
    }

    protected override void OnDead()
    {
        //model3DTr.DORotate(new Vector3(180, 0, 0), 0.1f);
        model3DTr.rotation = Quaternion.Euler(180 + transform.rotation.eulerAngles.x, 0, 0);
        minionInstance.CurrentHealthPoint = minionInstance.So.health;
        mapManager.GetWorldPosFromTilePos(new Vector2Int(indexX, indexY), out Vector3 worldPos);
        mapManager.RemoveEnemyOnTile(
            new Vector2Int(indexX, indexY), this, worldPos);
        isDigger = true;
        isReadyToUndig = false;
    }

    public void FinishToDig()
    {
        int index = 0;
        mapManager.AddMinionOnTile(
            new Vector2Int(indexX, indexY), this, ref index);
        model3DTr.DORotate(transform.rotation.eulerAngles, 0.1f);
    }
}