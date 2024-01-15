using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelCardScript : MonoBehaviour
{
    public Transform SpriteAttached;
    public Sprite lockedSprite;
    public Image BlackImage;
    public bool isLocked = false;
    public int biomeIndex;

    void Start()
    {
        if (DungeonManager._instance.dungeons[biomeIndex].isLocked)
        {
            SpriteAttached.GetComponent<SpriteRenderer>().sprite = lockedSprite;
            isLocked = true;
        }
    }
    
    private void OnMouseEnter()
    {
        if (!isLocked)
        {
            SpriteAttached.DOLocalMove(new Vector3(0, 0f, -1), 0.1f);
        }
        else
        {
            SpriteAttached.DOLocalRotate(new Vector3(0, 0, 5), 0.05f).SetLoops(4, LoopType.Yoyo);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            Debug.Log("Clicked");
            //DungeonManager._instance.SetSelectedBiome(biomeIndex);
            StartCoroutine(StartLevel());
        }
    }

    private void OnMouseExit()
    {
        SpriteAttached.DOLocalMove(new Vector3(0, 0f, 0f), 0.1f);
        SpriteAttached.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
    }

    IEnumerator StartLevel()
    {
        Camera.main.transform.DOMove(transform.position, 3);
        BlackImage.DOColor(Color.black, 1f);
        yield return new WaitForSeconds(0.95f);
        DungeonManager._instance.SetSelectedBiome(biomeIndex);
    }
    
}
