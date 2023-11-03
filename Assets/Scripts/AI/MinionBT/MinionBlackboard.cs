using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBlackboard : MonoBehaviour
{
    public MinionData minionData;
    public Vector2Int heroPosition;
    public DirectionToMove dir = DirectionToMove.None;
    
    

    public void GetHeroPos()
    {
        minionData.GetHeroPos();
    }
}
