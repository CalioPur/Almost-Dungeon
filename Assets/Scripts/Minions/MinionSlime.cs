using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSlime : MinionData
{
    [SerializeField] private MinionData slimePrefab;

    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }

    protected override void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        minionInstance.CurrentHealthPoint -= damage;
        if (minionInstance.CurrentHealthPoint <= 0)
        {
            isDead = true;
            OnDead();
        }
        else
        {
            //invocation new slime
            SpawnEnemyManager.SpawnEnemy(slimePrefab, new Vector2Int(indexX, indexY), transform.position, mapManager,
                true);
        }
    }


    protected override void Init()
    {
        base.Init();
        Hero.OnGivePosBackEvent += GetHeroPos;
    }
}