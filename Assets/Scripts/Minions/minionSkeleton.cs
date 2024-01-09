using UnityEngine;
using UnityEngine.UI;

public class minionSkeleton : MinionData
{
    [HideInInspector] public bool isDigger;
    [HideInInspector] public bool isReadyToUndig;
    
    [SerializeField] private Sprite spriteDig;
    [SerializeField] private Sprite spriteUndig;
    
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
        sprite.sprite = spriteDig;
        minionInstance.CurrentHealthPoint = minionInstance.So.health;
        mapManager.GetWorldPosFromTilePos(new Vector2Int(indexX, indexY) , out Vector3 worldPos);
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
        sprite.sprite = spriteUndig;
    }
}
