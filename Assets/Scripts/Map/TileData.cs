using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileData : MonoBehaviour
{
    private CardInfoInstance _instance;

    public bool hasDoorUp
    {
        get => _instance?.DoorOnTop ?? false;
        set => _instance.DoorOnTop = value;
    }

    public bool hasDoorRight
    {
        get => _instance?.DoorOnRight ?? false;
        set => _instance.DoorOnRight = value;
    }

    public bool hasDoorDown
    {
        get => _instance?.DoorOnBottom ?? false;
        set => _instance.DoorOnBottom = value;
    }

    public bool hasDoorLeft
    {
        get => _instance?.DoorOnLeft ?? false;
        set => _instance.DoorOnLeft = value;
    }

    public bool isConnectedToPath = false;

    public bool isExit = false;

    public bool isVisited = false;
    public bool PiecePlaced => _instance != null;


    [FormerlySerializedAs("minions")] [Header("Monsters")]
    public List<TrapData> enemies = new();

    [Header("Image")] public SpriteRenderer img;

    public void SetInstance(CardInfoInstance instance)
    {
        _instance = instance;
        _instance.OnRotationChangedEvent += UpdateAppearance;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (_instance == null) return;
        img.sprite = _instance.So.imgOnMap;
        transform.rotation = Quaternion.Euler(90, 0, _instance.Rotation);
    }
    
    public bool GetFirstAvailabalePosition(out Vector3 pos, out int index)
    {
        for (int i = 0; i < _instance.So.offsetSpawn.Length; i++)
        {
            if (_instance.offsetSpawnUsed[i]) continue;
            pos = _instance.So.offsetSpawn[i];
            _instance.offsetSpawnUsed[i] = true;
            index = i;
            return true;
        }

        pos = Vector3.zero;
        index = 0;
        return false;
    }
    
    public void freePosition(int index)
    {
        _instance.offsetSpawnUsed[index] = false;
    }
}