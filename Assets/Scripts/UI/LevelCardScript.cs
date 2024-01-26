using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCardScript : MonoBehaviour
{
    public SpriteRenderer SpriteAttached;
    public Sprite lockedSprite;
    public Sprite FinishedSprite;
    public Image BlackImage;
    public bool isLocked = false;
    public int biomeIndex;
    
    [SerializeField] private SoundManagerIngame soundManagerIngame;
    [SerializeField] private TMP_Text tryText;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text biomeName;

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
        else
        {
            SpriteAttached.transform.DOLocalRotate(new Vector3(0, 0, 5), 0.05f).SetLoops(4, LoopType.Yoyo);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isLocked)
        {
            Debug.Log("Clicked");
            soundManagerIngame.PlayDialogueSFX("UiClick");
            //DungeonManager._instance.SetSelectedBiome(biomeIndex);
            StartCoroutine(StartLevel());
        }
    }

    private void OnMouseExit()
    {
        SpriteAttached.transform.DOLocalMove(new Vector3(0, 0f, 0f), 0.1f);
        SpriteAttached.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
    }

    IEnumerator StartLevel()
    {
        Camera.main.transform.DOMove(transform.position, 3);
        BlackImage.DOColor(Color.black, 1f);
        yield return new WaitForSeconds(0.95f);
        DungeonManager._instance.SetSelectedBiome(biomeIndex);
        PlayerPrefs.SetInt("LevelTry" + biomeIndex, PlayerPrefs.GetInt("LevelTry" + biomeIndex, 0) + 1);
    }
    
}
