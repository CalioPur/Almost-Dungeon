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
    }

    protected override void Init()
    {
        webInstance = SO.CreateInstance();
    }
}