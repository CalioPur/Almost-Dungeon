using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AddButtonForLevelsSelection : MonoBehaviour
{
    [SerializeField] private ButtonPrefab buttonPrefab;
    [SerializeField] private DungeonManager dungeonManager;
    [SerializeField] private RectTransform rect;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Image CanvasBlack;    
    private List<GameObject> buttonList = new();
    
    [SerializeField] List<Sprite> dungeonsSprite;
    [SerializeField] Sprite lockedSprite;
    
    void OnEnable()
    {
        var sizeOfScreen = new Vector2(Screen.width, Screen.height);
        rect.DOMoveY(sizeOfScreen.y / 2, 0.5f).SetEase(Ease.Linear);
    }
    
    void SeeYouNextTime()
    {
        rect.DOMoveY(-800, 1f).SetEase(Ease.Linear);
    }
    
    void Start()
    {
        Time.timeScale = 1;
        int cpt = 0;
        foreach (var biome in dungeonManager.dungeons)
        {
            ButtonPrefab btnPrefab = Instantiate(buttonPrefab, transform);
            buttonList.Add(btnPrefab.gameObject);
            
            btnPrefab.Image.sprite = dungeonsSprite[cpt];
            btnPrefab.Image.color = Color.white;
            btnPrefab.Image.SetNativeSize();
            
            if (biome.isLocked)
            {
                btnPrefab.Image.sprite = lockedSprite;
                cpt++;
                continue;
            }
            
            
            int biomeIndex = cpt;
            btnPrefab.Btn.onClick.AddListener(() =>
            {
                buttonList.ForEach(button => button.SetActive(false));
                SeeYouNextTime();
                videoPlayer.playbackSpeed = 1;
                videoPlayer.Play();
                CanvasBlack.gameObject.SetActive(true);
                StartCoroutine(AlphaLerp(biomeIndex));
            });
            cpt++;
        }
        StopAllCoroutines();
    }

    public IEnumerator AlphaLerp(int biomeIndex)
    {
        
        var alpha = CanvasBlack.color.a;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            CanvasBlack.color = new Color(0, 0, 0, alpha);
            yield return null; // WTF why ??
        }
        dungeonManager.SetSelectedBiome(biomeIndex);
    }
}
