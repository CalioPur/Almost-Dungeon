using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSlime : MinionData
{
    [SerializeField] private MinionData slimePrefab;

    public override void TakeDamage(int damage, AttackType attackType)
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
                true, -1);
        }
    }
}