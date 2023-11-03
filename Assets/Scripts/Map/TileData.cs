using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileData : MonoBehaviour
{
    
    [SerializeField] private CardInfoInstance _instance;
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

    [Header("Occupation")]
    public bool isPlayerhere = false;
    public bool isMonsterhere = false;
    public bool isChesthere = false;
    public bool isTrapHere = false;
    public bool PiecePlaced => _instance != null;
    

    [FormerlySerializedAs("minions")] [Header("Monsters")]
    public List<TrapData> enemies = new ();
    /*public List<Hero> bigMinion = new ();
    public List<Hero> archerMinion = new ();*/
    
    [Header("Image")]
    public SpriteRenderer img;
    
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (hasDoorUp) Gizmos.DrawSphere(transform.position + Vector3.forward * 0.4f, 0.1f);
        if (hasDoorRight) Gizmos.DrawSphere(transform.position + Vector3.right * 0.4f, 0.1f);
        if (hasDoorDown) Gizmos.DrawSphere(transform.position + Vector3.back * 0.4f, 0.1f);
        if (hasDoorLeft) Gizmos.DrawSphere(transform.position + Vector3.left * 0.4f, 0.1f);
    }

}

