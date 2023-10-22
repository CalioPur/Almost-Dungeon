using UnityEngine;
using UnityEngine.UI;

public class TileData : MonoBehaviour
{
    [Header("Doors")]
    public bool hasDoorUp;
    public bool hasDoorRight;
    public bool hasDoorDown;
    public bool hasDoorLeft;

    [Header("Occupation")]
    public bool isPlayerhere = false;
    public bool isMonsterhere = false;
    public bool isChesthere = false;
    public bool isTrapHere = false;
    public bool PiecePlaced = false;

    [Header("Monsters")]
    public int lilMinion;
    public int bigMinion;
    public int archerMinion;
    
    [Header("Image")]
    public SpriteRenderer img;

    public void RotateDoors90()
    {
        bool temp = hasDoorLeft;
        hasDoorLeft = hasDoorDown;
        hasDoorDown = hasDoorRight;
        hasDoorRight = hasDoorUp;
        hasDoorUp = temp;
    }
}
