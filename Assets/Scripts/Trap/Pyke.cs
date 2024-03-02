using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Pyke : TrapData
{
    [SerializeField] ParticleSystem fireParticles;
    private EnemyInstance pykeInstance;
    private Vector2Int heroPos = new Vector2Int(-9999, -9999);
    
    public static event Action<int> DealDamageEvent;
    
    public override void TakeDamage(int damage, AttackType attackType)
    {
        Debug.LogError("Pyke TakeDamage ! is not normal");
    }
    
    private IEnumerator AttackFX()
    {
        Vector3 pos = fireParticles.transform.position;
        fireParticles.transform.position = new Vector3(pos.x, pos.y - 0.15f, pos.z);
        fireParticles.gameObject.SetActive(true);
        fireParticles.gameObject.transform.DOMoveY(pos.y, TickManager.Instance.calculateBPM()).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(TickManager.Instance.calculateBPM());
        fireParticles.gameObject.SetActive(false);
    }

    protected override void OnTick()
    {
        bool isOnFire = false;
        //je check si le hero ou les minions sont sur ma case
        mapManager.GetMonstersOnPos(new Vector2Int(indexX, indexY), out List<TrapData> minions);
        GetHeroPosOnTile(Hero.Instance.GetIndexHeroPos());
        if (minions.Count > 0)
        {
            foreach (var minion in minions)
            {
                minion.TakeDamage(pykeInstance.So.damage, AttackType.Fire);
            }
            isOnFire = true;
        }
        if (heroPos.x == indexX && heroPos.y == indexY)
        {
            Attack(pykeInstance.So.damage, AttackType.Fire, 0.0f);
            DealDamageEvent?.Invoke(pykeInstance.So.damage);
            isOnFire = true;
        }

        print("HeroPos : " + heroPos);
        print("MyPos : " + indexX + " " + indexY);
        if (isOnFire)
        {
            StartCoroutine(AttackFX());
        }
    }

    protected override void Init()
    {
        pykeInstance = SO.CreateInstance();
        TickManager.Instance.SubscribeToMovementEvent(MovementType.Trap, OnTick, out entityId);
        
    }

    private void GetHeroPosOnTile(Vector2Int pos)
    {
        heroPos = pos;
    }

    private void OnDisable()
    {
        TickManager.Instance.UnsubscribeFromMovementEvent(MovementType.Trap, entityId);
    }

    protected override void OnDead()
    {
        Debug.LogError("Pyke is dead ! is not normal");
        
    }
}