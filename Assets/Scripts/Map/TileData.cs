using UnityEngine;
using UnityEngine.UI;

public class TileData : MonoBehaviour
{
    
    [SerializeField] private CardInfoInstance _instance;
    public bool hasDoorUp => _instance?.DoorOnTop ?? false;
    public bool hasDoorRight => _instance?.DoorOnRight ?? false;
    public bool hasDoorDown => _instance?.DoorOnBottom ?? false;
    public bool hasDoorLeft => _instance?.DoorOnLeft ?? false;

    [Header("Occupation")]
    public bool isPlayerhere = false;
    public bool isMonsterhere = false;
    public bool isChesthere = false;
    public bool isTrapHere = false;
    public bool PiecePlaced => _instance != null;

    [Header("Monsters")]
    public int lilMinion;
    public int bigMinion;
    public int archerMinion;
    
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
}
