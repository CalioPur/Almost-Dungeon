using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public void Move(DirectionToMove directionToMove)
    {
        Vector3 pos = Vector3.zero;
        switch (directionToMove)
        {
            case DirectionToMove.Up:
                mapManager.GetWorldPosFromTilePos(indexMinionX, indexMinionY + 1, out pos);
                indexMinionY++;
                break;
            case DirectionToMove.Down:
                mapManager.GetWorldPosFromTilePos(indexMinionX, indexMinionY - 1, out pos);
                indexMinionY--;
                break;
            case DirectionToMove.Left:
                mapManager.GetWorldPosFromTilePos(indexMinionX - 1, indexMinionY, out pos);
                indexMinionX--;
                break;
            case DirectionToMove.Right:
                mapManager.GetWorldPosFromTilePos(indexMinionX + 1, indexMinionY, out pos);
                indexMinionX++;
                break;
        }

        tr.DOMove(pos + new Vector3(1, 0.1f, 1), 0.5f);
    }
}
