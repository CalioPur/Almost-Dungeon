using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeroBlackboard : MonoBehaviour
{
    public Personnalities personality;
    [HideInInspector] public Hero hero;
    [HideInInspector] public DirectionToMove directionToMove = DirectionToMove.None;
    [HideInInspector] public List<TrapData> Targets = new();
    [HideInInspector] public List<TrapData> ChosenTarget = new();
    [HideInInspector] public List<Vector2Int> DoorSaw = new();
    [HideInInspector] public Vector2Int DoorTarget;
}