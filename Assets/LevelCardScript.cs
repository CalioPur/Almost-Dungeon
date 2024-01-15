using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class LevelCardScript : MonoBehaviour
{
    public Transform SpriteAttached;
    public int biomeIndex;
    private void OnMouseEnter()
    {
        SpriteAttached.DOLocalMove(new Vector3(0, 0f, -1), 0.1f);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            DungeonManager._instance.SetSelectedBiome(biomeIndex);
        }
    }

    private void OnMouseExit()
    {
        SpriteAttached.DOLocalMove(new Vector3(0, 0f, 0f), 0.1f);
    }


    
}
