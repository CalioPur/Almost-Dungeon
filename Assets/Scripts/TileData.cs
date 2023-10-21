using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    [Header("Doors")]
    public bool hasDoorUp;
    public bool hasDoorRight;
    public bool hasDoorDown;
    public bool hasDoorLeft;

    [Header("Player")]
    public bool isPlayerhere;

    [Header("Monsters")]
    public int lilMinion;
    public int bigMinion;
    public int archerMinion;

    public void RotateDoors90()
    {
        bool temp = hasDoorLeft;
        hasDoorLeft = hasDoorDown;
        hasDoorDown = hasDoorRight;
        hasDoorRight = hasDoorUp;
        hasDoorUp = temp;
    }
}
