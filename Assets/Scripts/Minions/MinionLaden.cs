using System;
using System.Collections;
using UnityEngine;

public class MinionLaden : MinionData
{
    [SerializeField] private int damageFire = 5;
    [SerializeField] private ParticleSystem ExplosionFX;
    private AttackType DeadAttackType = AttackType.Fire;

    private void ExplosionAttack(Vector2Int pos)
    {
        mapManager.GetMonstersOnPos(pos, out var monsters);
        foreach (var monster in monsters)
        {
            monster.TakeDamage(damageFire, AttackType.Fire);
            if (monster is IFlippable flip)
            {
                flip.Flip();
            }
        }

        if (bt.blackboard.heroPosition == pos)
        {
            Attack(damageFire, AttackType.Fire);
            if (Hero.Instance is IFlippable flip)
            {
                flip.Flip();
            }
        }
    }

    public override void TakeDamage(int damage, AttackType attackType)
    {
        StartCoroutine(TakeDamageFX(damage, attackType));
    }

    IEnumerator TakeDamageFX(int damage, AttackType attackType)
    {
        DeadAttackType = attackType;
        if (attackType == AttackType.Fire)
        {
            yield return new WaitForSeconds(0.5f);
            mapManager.RemoveEnemyOnTile(new Vector2Int(indexX, indexY), this, transform.position);
            if (mapManager.HasDoorOpen(new Vector2Int(indexX, indexY), new Vector2Int(indexX, indexY + 1)))
            {
                ExplosionAttack(new Vector2Int(indexX, indexY + 1));
            }

            if (mapManager.HasDoorOpen(new Vector2Int(indexX, indexY), new Vector2Int(indexX, indexY - 1)))
            {
                ExplosionAttack(new Vector2Int(indexX, indexY - 1));
            }

            if (mapManager.HasDoorOpen(new Vector2Int(indexX, indexY), new Vector2Int(indexX + 1, indexY)))
            {
                ExplosionAttack(new Vector2Int(indexX + 1, indexY));
            }

            if (mapManager.HasDoorOpen(new Vector2Int(indexX, indexY), new Vector2Int(indexX - 1, indexY)))
            {
                ExplosionAttack(new Vector2Int(indexX - 1, indexY));
            }
        }

        base.TakeDamage(damage, attackType);
    }
    
    protected override void OnDead()
    {
        if (DeadAttackType == AttackType.Fire)
            Destroy(Instantiate(ExplosionFX, transform.position, ExplosionFX.transform.rotation), 1.5f);

        base.OnDead();
    }
}