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

public class HeroSOInstance
{
    public HeroSO so { get; }
    public string nameOfHero;
    public int health;
    public float speed;
    public HeroesInfo classe;
    public VisionType visionType;
    public Aggressivity aggressivity;
    public List<Personnalities> personalities;
    public Language[] languages;
    public string keyToUnlock;
    
    public HeroSOInstance(HeroSO so)
    {
        this.so = so;
        this.nameOfHero = so.nameOfHero;
        this.health = so.health;
        this.speed = so.speed;
        this.classe = so.classe;
        this.visionType = so.visionType;
        this.aggressivity = so.aggressivity;
        this.personalities = so.personalities;
        this.languages = so.languages;
        this.keyToUnlock = so.keyToUnlock;
    }
}