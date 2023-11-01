using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionData : MonoBehaviour
{
    public static event Action OnHeroMoved;

    public int indexMinionX;
    public int indexMinionY;
    protected int entityId;
    public MapManager mapManager;
   
    [SerializeField] protected Transform tr;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected MinionBTBase bt;
    
    public void GetHeroPos()
    {
        OnHeroMoved?.Invoke();
    }
    
}
