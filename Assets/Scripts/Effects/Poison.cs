using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Poison", menuName = "ScriptableObjects/ActiveEffect/Poison", order = 1)]
public class Poison : ActiveEffect
{
    public override void ApplyEffect(Hero hero)
    {
        Debug.Log("tu es empoisonné bro");
    }
}
