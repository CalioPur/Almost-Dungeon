using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HeroSO", menuName = "ScriptableObjects/CreationDungeonTool/LevelComponents/HeroSO", order = 9)]
public class HeroSO : ScriptableObject
{
    public string nameOfHero;
    public int health;
    public int speed;//vitesse ?
    public HeroesInfo classe;
    public List<Personnalities> personnalities;
    public List<TextAsset> heroDialogues;
    public string keyToUnlock;
}
