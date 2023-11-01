using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileData : MonoBehaviour
{
    
    [SerializeField] private CardInfoInstance _instance;
    public bool hasDoorUp => _instance?.DoorOnTop ?? false;
    public bool hasDoorRight => _instance?.DoorOnRight ?? false;
    public bool hasDoorDown => _instance?.DoorOnBottom ?? false;
    public bool hasDoorLeft => _instance?.DoorOnLeft ?? false;
    
    public bool isConnectedToPath = false;

    [Header("Occupation")]
    public bool isPlayerhere = false;
    public bool isMonsterhere = false;
    public bool isChesthere = false;
    public bool isTrapHere = false;
    public bool PiecePlaced => _instance != null;

    [Header("Monsters")]
    public List<Hero> lilMinions = new ();
    public List<Hero> bigMinion = new ();
    public List<Hero> archerMinion = new ();
    
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
        img.sprite = _instance.So.img;
        transform.rotation = Quaternion.Euler(90, 0, _instance.Rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (hasDoorUp) Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 0.3f);
        if (hasDoorRight) Gizmos.DrawLine(transform.position, transform.position + Vector3.right* 0.3f);
        if (hasDoorDown) Gizmos.DrawLine(transform.position, transform.position + Vector3.back* 0.3f);
        if (hasDoorLeft) Gizmos.DrawLine(transform.position, transform.position + Vector3.left* 0.3f);
    }

}

