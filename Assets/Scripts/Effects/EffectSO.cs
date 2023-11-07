using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effect", order = 1)]
public class EffectSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public ActiveEffect[] Effects { get; private set; }
    
    public EffectInstance CreateInstance()
    {
        return new EffectInstance(this);
    }
}

public class EffectInstance
{
    public EffectSO So { get; }
    public List<ActiveEffectInstance> EffectsInstances { get; private set; }
    
    public EffectInstance(EffectSO effectSo)
    {
        So = effectSo;
        EffectsInstances = new ();
        for (int i = 0; i < effectSo.Effects.Length; i++)
        {
            EffectsInstances.Add(effectSo.Effects[i].CreateInstance());
        }
    }
    
    public void Tick()
    {
        for (int i = 0; i < EffectsInstances.Count; i++)
        {
            //if (EffectsInstances[i].Tick())
            {
                //EffectsInstances[i].So.RemoveEffect();
                EffectsInstances.RemoveAt(i);
                i--;
            }
        }
    }
}