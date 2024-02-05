using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public struct Langue{
    public List<TextAsset> heroDialogues;
}
[CreateAssetMenu(fileName = "HeroSO", menuName = "ScriptableObjects/CreationDungeonTool/LevelComponents/HeroSO", order = 9)]
public class HeroSO : ScriptableObject
{
    public string nameOfHero;
    public int health;
    public int speed;//vitesse ?
    public HeroesInfo classe;
    public VisionType visionType;
    public Aggressivity aggressivity;
    public List<PersonnalitiesV2> personalities;
    public Langue[] langues;
    public string keyToUnlock;
}
