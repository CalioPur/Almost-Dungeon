using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBlackboard : MonoBehaviour
{
    [HideInInspector] public MinionData minionData;
    [HideInInspector] public Vector2Int heroPosition;
    [HideInInspector] public DirectionToMove dir = DirectionToMove.None;
}
