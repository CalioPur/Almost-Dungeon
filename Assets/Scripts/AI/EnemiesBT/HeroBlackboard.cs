using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeroBlackboard : MonoBehaviour
{
    [HideInInspector] public Hero hero;
    [HideInInspector] public DirectionToMove directionToMove = DirectionToMove.None;
    [HideInInspector] public List<Hero> Targets = new ();
    [HideInInspector] public Hero ChosenTarget;
}
