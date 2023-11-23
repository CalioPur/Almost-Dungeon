using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeroBlackboard : MonoBehaviour
{
    [HideInInspector] public Hero hero;
     public Personnalities personality;
    [HideInInspector] public DirectionToMove directionToMove = DirectionToMove.None;
    [HideInInspector] public List<TrapData> Targets = new ();
    [HideInInspector] public TrapData ChosenTarget;
}
