using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AddButtonForLevelsSelection : MonoBehaviour
{
    public GameObject buttonPrefab;
    public DungeonManager dungeonManager;
    [SerializeField] private Image image;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject CanvasBlack;
    [SerializeField] float timeToWait = 2f;
    
    private List<GameObject> buttonList = new();
    void Start()
    {
        int cpt = 0;
        foreach (var biome in dungeonManager.dungeons)
        {
            GameObject theButton = Instantiate(buttonPrefab, transform);
            buttonList.Add(theButton);
            theButton.GetComponentInChildren<TMP_Text>().text = biome.name;
            int biomeIndex = cpt;
            theButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                buttonList.ForEach(button => button.SetActive(false));
                image.color = Color.clear;
                videoPlayer.playbackSpeed = 1;
                videoPlayer.Play();
                CanvasBlack.SetActive(true);
                StartCoroutine(AlphaLerp(biomeIndex));
            });
            cpt++;
        }
    }
    private IEnumerator AlphaLerp(int biomeIndex)
    {
        var alpha = CanvasBlack.GetComponent<Image>().color.a;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            CanvasBlack.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        dungeonManager.SetSelectedBiome(biomeIndex);
        yield break;
    }
}
