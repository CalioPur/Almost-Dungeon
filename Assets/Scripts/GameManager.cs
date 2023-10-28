using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite imgPlayer;
    [FormerlySerializedAs("createMap")] [SerializeField] private MapManager mapManager;
    [SerializeField] private CardInfo enterDungeonInfo;
    [SerializeField] private SpriteRenderer heroRenderer;
    
    private Transform heroTr;
    
    // Start is called before the first frame update
    void Start()
    {
        mapManager.InitMap();
        Vector3 pos = mapManager.InitEnterDungeon(enterDungeonInfo);
        heroTr = Instantiate(heroRenderer.gameObject, pos, heroRenderer.transform.rotation).transform;
        heroTr.GetComponent<SpriteRenderer>().sprite = imgPlayer;
    }
}
