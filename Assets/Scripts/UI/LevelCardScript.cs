using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelCardScript : MonoBehaviour
{
    public SpriteRenderer SpriteAttached;
    public Sprite lockedSprite;
    public Sprite normalSprite;
    public Sprite FinishedSprite;
    public Image BlackImage;
    public bool isLocked = false;
    public int biomeIndex;
    
    [SerializeField] private SoundManagerIngame soundManagerIngame;
    [SerializeField] private TMP_Text tryText;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text biomeName;
    
    private bool canPlay = true;
    private bool shouldPlay = false;

    void Start()
    {
        if (DungeonManager._instance.dungeons[biomeIndex].isLocked && (PlayerPrefs.GetInt("LevelUnlock" + biomeIndex, 0)==0))
        {
            SpriteAttached.sprite = lockedSprite;
            foreach (Transform child in SpriteAttached.transform)
            {
                child.gameObject.SetActive(false);
            }
            isLocked = true;
            if (PlayerPrefs.GetInt("LevelJustUnlock" + biomeIndex, 0) == 1)
            {
                shouldPlay = true;
            }
        }
        else if (PlayerPrefs.GetInt("LevelBeaten" + biomeIndex,0) == 1)
        {
            SpriteAttached.sprite = FinishedSprite;
        }
        
        tryText.text = PlayerPrefs.GetInt("LevelTry" + biomeIndex, 0).ToString();
        victoryText.text = PlayerPrefs.GetInt("LevelVictory" + biomeIndex, 0).ToString();
        biomeName.text = DungeonManager._instance.dungeons[biomeIndex].name;
    }
    
    private void OnMouseEnter()
    {
        soundManagerIngame.PlayDialogueSFX("UiNegativeClick");
        
        if (!isLocked)
        {
            SpriteAttached.transform.DOLocalMove(new Vector3(0, 0f, -1), 0.1f);
        }
        else if (PlayerPrefs.GetInt("LevelJustUnlock" + biomeIndex, 0) == 1)
        {
            //on fait rien
        }
        else
        {
            SpriteAttached.transform.DOLocalRotate(new Vector3(0, 0, 5), 0.05f).SetLoops(4, LoopType.Yoyo);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            soundManagerIngame.PlayDialogueSFX("UiClick");
            //DungeonManager._instance.SetSelectedBiome(biomeIndex);
            
            StartCoroutine(StartLevel());
        }
        else if (Input.GetMouseButtonDown(0) && isLocked && PlayerPrefs.GetInt("LevelJustUnlock" + biomeIndex, 0) == 1)
        {
            
            shouldPlay = false;
            canPlay = true;
            if(canPlay) StartCoroutine(UnlockAnim());
        }
    }

    private void OnMouseExit()
    {
        SpriteAttached.transform.DOLocalMove(new Vector3(0, 0f, 0f), 0.1f);
        SpriteAttached.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
    }

    private void Update()
    {
        if(shouldPlay && canPlay)
        {
            StartCoroutine(UnlockAnimPump());
        }
    }

    IEnumerator StartLevel()
    {
        Camera.main.transform.DOMove(transform.position, 3);
        BlackImage.DOColor(Color.black, 1f);
        yield return new WaitForSeconds(0.95f);
        DungeonManager._instance.SetSelectedBiome(biomeIndex);
        PlayerPrefs.SetInt("LevelTry" + biomeIndex, PlayerPrefs.GetInt("LevelTry" + biomeIndex, 0) + 1);
    }
    
    IEnumerator UnlockAnimPump()
    {
        canPlay = false;
        SpriteAttached.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(1.1f);
        canPlay = true;
    }
    IEnumerator UnlockAnim()
    {
        canPlay = false;
        Transform tf = SpriteAttached.transform;
        
        //go up, do a rotation, change sprite mid rot, go down
        //reset scale
        tf.DOScale(new Vector3(1, 1, 1), 0.1f);
        
        tf.DOLocalMoveZ(-1f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        tf.DOLocalRotate(new Vector3(0, 180, 0), 0.1f);
        yield return new WaitForSeconds(0.1f);
        
        foreach (Transform child in tf)
        {
            child.gameObject.SetActive(true);
        }
        tryText.text = PlayerPrefs.GetInt("LevelTry" + biomeIndex, 0).ToString();
        victoryText.text = PlayerPrefs.GetInt("LevelVictory" + biomeIndex, 0).ToString();
        biomeName.text = DungeonManager._instance.dungeons[biomeIndex].name;
        SpriteAttached.sprite = normalSprite;
        
        
        tf.DOLocalRotate(new Vector3(0, 360, 0), 0.1f);
        yield return new WaitForSeconds(0.3f);
        tf.DOLocalMoveZ(0f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        PlayerPrefs.SetInt("LevelUnlock" + biomeIndex, 1);
        canPlay = true;
        isLocked = false;
    }
}
