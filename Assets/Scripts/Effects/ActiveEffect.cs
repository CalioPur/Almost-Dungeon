using System;
using UnityEngine;

[Serializable]
public abstract class ActiveEffect : ScriptableObject
{
    public abstract void ApplyEffect(Hero hero);
}