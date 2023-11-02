using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class MinionData : MonoBehaviour
{
    public static event Action OnHeroMoved;

    public bool isDead;
    public int indexMinionX;
    public int indexMinionY;
    protected int entityId;
    public MapManager mapManager;
    
   
    [SerializeField] protected Transform tr;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected MinionBTBase bt;
    [SerializeField] private minionSOScript minionSO;
    
    private MinionInstance minionInstance;
    
    protected abstract void MinionDie();
    
    public void GetHeroPos()
    {
        OnHeroMoved?.Invoke();
    }

    public void Move(Vector3 pos)
    {
        
        tr.DOMove(pos + new Vector3(1, 0.1f, 1), 0.5f);
    }

    public void TakeDamage(int soAttackPoint)
    {
        if (isDead) return;
        
        Debug.Log("ENNEMI TAKE DAMAGE");
        minionInstance.CurrentHealthPoint-=soAttackPoint;
        if(minionInstance.CurrentHealthPoint<=0)
        {
            isDead = true;
            MinionDie();
        }
        // bt.blackboard.health -= soAttackPoint;
        // if (bt.blackboard.health <= 0)
        // {
        //     isDead = true;
        //     sprite.color = Color.red;
        //     bt.blackboard.health = 0;
        // }
    }

    protected void Init()
    {
        minionInstance = minionSO.CreateInstance();
    }
}
