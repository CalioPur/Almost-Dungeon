using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AddButtonForLevelsSelection : MonoBehaviour
{
    public GameObject buttonPrefab;
    public DungeonManager dungeonManager;
    
    void Start()
    {
        int cpt = 0;
        foreach (var biome in dungeonManager.dungeons)
        {
            GameObject theButton = Instantiate(buttonPrefab, transform);
            theButton.GetComponentInChildren<TMP_Text>().text = biome.name;
            int biomeIndex = cpt;
            theButton.GetComponent<Button>().onClick.AddListener(() => dungeonManager.SetSelectedBiome(biomeIndex));
            cpt++;
        }
    }
}
