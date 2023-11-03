using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : TrapData
{
    private EnemyInstance webInstance;

    private void Start()
    {
        Init();
    }

    
    
    public override void TakeDamage(int damage)
    {
        webInstance.CurrentHealthPoint -= damage;
        if (webInstance.CurrentHealthPoint <= 0)
        {
            OnDead();
        }
    }

    protected override void OnTick()
    {
        //je check si le hero ou les minions sont sur ma case
    }

    protected override void Init()
    {
        webInstance = SO.CreateInstance();
    }

    protected override void OnDead()
    {
        isDead = true;
        mapManager.RemoveEnemyOnTile(new Vector2Int(indexMinionX, indexMinionY), this);
        Destroy(gameObject);
    }

    public override void Attack()
    {
        //TODO: Attack
    }
}