using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effect", order = 1)]
public class EffectSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int NbTurnToEffect { get; private set; }
    [field: SerializeField] public ActiveEffect[] Effects { get; private set; }
    
    public EffectInstance CreateInstance()
    {
        return new EffectInstance(this);
    }
}

public class EffectInstance
{
    public EffectSO So { get; }
    public int RemainingTurnToEffect { get; private set; }
    public ActiveEffect[] Effects { get; private set; }
    
    public EffectInstance(EffectSO effectSo)
    {
        So = effectSo;
        RemainingTurnToEffect = effectSo.NbTurnToEffect;
        Effects = new ActiveEffect[effectSo.Effects.Length];
        for (int i = 0; i < effectSo.Effects.Length; i++)
        {
            //Effects[i] = EffectsFactoryManager.Create(effectSo.Effects[i]);
        }
    }

    public void Tick()
    {
        RemainingTurnToEffect--;
    }
}