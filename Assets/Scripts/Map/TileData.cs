using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileData : MonoBehaviour
{
    public CardInfoInstance _instance { get; private set; }

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

    public bool IsVisited
    {
        get { return isVisited; }
        set
        {
            isVisited = value;
            img.color = isVisited ? new Color(1f, 1f, 1f) : new Color(0.35f, 0.35f, 0.35f);
        }
    }

    public bool isVisited;


    public bool isRoom = false;
    public bool PiecePlaced => _instance != null;

    [FormerlySerializedAs("minions")] [Header("Monsters")]
    public List<TrapData> enemies = new();

    [Header("Image")] public SpriteRenderer img;

    public void SetInstance(CardInfoInstance instance)
    {
        if (instance == null)
        {
            _instance = null;
            img.sprite = null;
            return;
        }

        _instance = new CardInfoInstance(instance.So);
        _instance.CopyValues(instance);
        _instance.OnRotationChangedEvent += UpdateAppearance;
        UpdateAppearance();
    }

    private void OnDisable()
    {
        if (_instance == null) return;
        _instance.OnRotationChangedEvent -= UpdateAppearance;
        _instance.OnDisable();
    }

    private void UpdateAppearance()
    {
        if (_instance == null) return;
        img.sprite = _instance.So.imgOnMap;
        transform.rotation = Quaternion.Euler(90, 0, _instance.Rotation);
        img.color = new Color(0.35f, 0.35f, 0.35f);
    }

    public bool AvailableForSpawn()
    {
        return (!_instance.offsetSpawnUsed);
    }

    public bool GetFirstAvailabalePosition()
    {
        if (_instance == null || _instance.offsetSpawnUsed) return false;
        _instance.offsetSpawnUsed = true;
        return true;
    }

    public void freePosition()
    {
        _instance.offsetSpawnUsed = false;
    }

    public void SnapToGrid()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y,
            Mathf.Round(transform.position.z));
    }
}