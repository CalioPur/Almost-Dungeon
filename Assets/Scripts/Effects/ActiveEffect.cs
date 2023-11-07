using System;
using UnityEngine;

[Serializable]
public abstract class ActiveEffect : ScriptableObject
{
    [field: SerializeField] public int NbTurnToEffect { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    public abstract void ApplyEffect(Hero hero);
    public abstract void RemoveEffect(Hero hero);
    public abstract void Tick(Hero hero);
    
    public ActiveEffectInstance CreateInstance()
    {
        return new ActiveEffectInstance(this);
    }
}

public class ActiveEffectInstance
{
    public ActiveEffect So { get; }
    public int RemainingTurnToEffect { get; private set; }
    public ActiveEffect[] Effects { get; private set; }
    
    public ActiveEffectInstance(ActiveEffect effectSo)
    {
        So = effectSo;
        RemainingTurnToEffect = effectSo.NbTurnToEffect;
    }

    public bool Tick(Hero hero)
    {
        So.Tick(hero);
        return --RemainingTurnToEffect <= 0;
    }
}