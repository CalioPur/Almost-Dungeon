using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Language
{
    public List<TextAsset> heroDialogues;
}

[CreateAssetMenu(fileName = "HeroSO", menuName = "ScriptableObjects/CreationDungeonTool/LevelComponents/HeroSO", order = 9)]
public class HeroSO : ScriptableObject
{
    public string nameOfHero;
    public int health;
    public float speed;//vitesse ?
    public HeroesInfo classe;
    public VisionType visionType;
    public Aggressivity aggressivity;
    public List<Personnalities> personalities;
    public Language[] languages;
    public string keyToUnlock;
}
