using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroesInfo", menuName = "ScriptableObjects/HeroesInfo", order = 1)]
public class HeroesInfo : ScriptableObject
{
  [field:SerializeField] public Sprite Img { get; private set; }
  [field:SerializeField] public int HealthPoint { get; private set; }
  [field:SerializeField] public int AttackPoint { get; private set; }
  [field:SerializeField] public bool IsAOE { get; private set; }
  [field:SerializeField] public int Range { get; private set; }
  [field:SerializeField] public Hero prefab { get; private set; }
  
  public HeroInstance CreateInstance()
  {
    return new HeroInstance(this);
  }
}

public class HeroInstance
{
  public HeroesInfo So { get;}
  
  public int CurrentHealthPoint;
  
  public HeroInstance(HeroesInfo info)
  {
    So = info;
    CurrentHealthPoint = info.HealthPoint;
  }
  
}

