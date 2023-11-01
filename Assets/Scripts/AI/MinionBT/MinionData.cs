using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionData : MonoBehaviour
{
    public int indexMinionX { get; private set; }
    public int indexMinionY { get; private set; }
    private int entityId;
    public MapManager mapManager { get; private set; }
    [SerializeField] private Transform tr;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private MinionBTBase bt;
    
}
