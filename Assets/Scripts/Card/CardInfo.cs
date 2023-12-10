using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "ScriptableObjects/CardInfo", order = 1)]
public class CardInfo : ScriptableObject
{
    [Header("Visual")]
    public Sprite imgOnHand;
    public Sprite imgOnMap;
    
    [Header("Minions parameters")]
    public EnemiDataOnHand[] TypeOfTrapOrEnemyToSpawn;
    public Vector3[] offsetMinionPos;
    
    [Header("Doors")]
    public bool DoorOnTop;
    public bool DoorOnBottom;
    public bool DoorOnLeft;
    public bool DoorOnRight;
    
    [Header("Others")]
    public string description;
    public int Rotation;
    public List<ItemDrop> ItemDrops { get; private set; } = new ();
    
    public CardInfoInstance CreateInstance()
    {
        return new CardInfoInstance(this);
    }

    public void CopyValues(CardInfo cardSo)
    {
        imgOnHand = cardSo.imgOnHand;
        imgOnMap = cardSo.imgOnMap;
        TypeOfTrapOrEnemyToSpawn = cardSo.TypeOfTrapOrEnemyToSpawn;
        offsetMinionPos = cardSo.offsetMinionPos;
        DoorOnTop = cardSo.DoorOnTop;
        DoorOnBottom = cardSo.DoorOnBottom;
        DoorOnLeft = cardSo.DoorOnLeft;
        DoorOnRight = cardSo.DoorOnRight;
        description = cardSo.description;
        Rotation = cardSo.Rotation;
        ItemDrops = cardSo.ItemDrops;
    }
}
public class CardInfoInstance
{
    public CardInfo So { get;}
    public int Rotation;
    public bool DoorOnTop { get;  set; }
    public bool DoorOnBottom { get; set; }
    public bool DoorOnLeft { get;  set; }
    public bool DoorOnRight { get;  set; }
    
    public EnemiDataOnHand[] TypeOfTrapOrEnemyToSpawnInstance { get;  private set; }
    
    public bool[] offsetSpawnUsed;
    
    public event Action OnRotationChangedEvent;
    
    public List<ItemDrop> ItemDrops { get; set; }
    
    public CardInfoInstance(CardInfo info)
    {
        So = info;
        Rotation = info.Rotation;
        DoorOnTop = info.DoorOnTop;
        DoorOnBottom = info.DoorOnBottom;
        DoorOnLeft = info.DoorOnLeft;
        DoorOnRight = info.DoorOnRight;
        offsetSpawnUsed = new bool[So.offsetMinionPos.Length];
        TypeOfTrapOrEnemyToSpawnInstance = new EnemiDataOnHand[So.TypeOfTrapOrEnemyToSpawn.Length];
        for (int i = 0; i < So.TypeOfTrapOrEnemyToSpawn.Length; i++)
        {
            TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile = So.TypeOfTrapOrEnemyToSpawn[i].indexOffsetTile;
            TypeOfTrapOrEnemyToSpawnInstance[i].type = So.TypeOfTrapOrEnemyToSpawn[i].type;
        }
        for (int i = 0; i < So.offsetMinionPos.Length; i++)
        {
            offsetSpawnUsed[i] = false;
        }
        
        ItemDrops = new List<ItemDrop>();
        for (int i = 0; i < info.ItemDrops.Count; i++)
        {
            ItemDrops.Add(info.ItemDrops[i]);
        }
    }
    
    public void AddRotation(bool NotClockwise)
    {
        Rotation += 90 * (NotClockwise ? 1 : -1);
        if (Rotation < 0) Rotation = 270;
        if (Rotation >= 360) Rotation = 0;

        bool Tmp = DoorOnTop; // counter-clockwise rotation

        
        if (NotClockwise)
        {
            DoorOnTop = DoorOnRight;
            DoorOnRight = DoorOnBottom;
            DoorOnBottom = DoorOnLeft;
            DoorOnLeft = Tmp;
            for (int i = 0; i < TypeOfTrapOrEnemyToSpawnInstance.Length; i++)
            {
                TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile--;
                if (TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile < 0)
                {
                    TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile = So.offsetMinionPos.Length - 1;
                }
            }
        }
        else
        {
            DoorOnTop = DoorOnLeft;
            DoorOnLeft = DoorOnBottom;
            DoorOnBottom = DoorOnRight;
            DoorOnRight = Tmp;
            for (int i = 0; i < TypeOfTrapOrEnemyToSpawnInstance.Length; i++)
            {
                TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile++;
                if (TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile >= So.offsetMinionPos.Length)
                {
                    TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile = 0;
                }
            }
        }
        
        OnRotationChangedEvent?.Invoke();
    }

    public void OnDisable()
    {
        OnRotationChangedEvent = null;
    }

    public void CopyValues(CardInfoInstance instance)
    {
        Rotation = instance.Rotation;
        DoorOnTop = instance.DoorOnTop;
        DoorOnBottom = instance.DoorOnBottom;
        DoorOnLeft = instance.DoorOnLeft;
        DoorOnRight = instance.DoorOnRight;
        offsetSpawnUsed = new bool[So.offsetMinionPos.Length];
        for (int i = 0; i < So.offsetMinionPos.Length; i++)
        {
            offsetSpawnUsed[i] = instance.offsetSpawnUsed[i];
        }
        TypeOfTrapOrEnemyToSpawnInstance = new EnemiDataOnHand[So.TypeOfTrapOrEnemyToSpawn.Length];
        for (int i = 0; i < So.TypeOfTrapOrEnemyToSpawn.Length; i++)
        {
            TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile = instance.TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile;
            TypeOfTrapOrEnemyToSpawnInstance[i].type = instance.TypeOfTrapOrEnemyToSpawnInstance[i].type;
        }
        ItemDrops = new List<ItemDrop>();
        for (int i = 0; i < instance.ItemDrops.Count; i++)
        {
            ItemDrops.Add(instance.ItemDrops[i]);
        }
    }
}