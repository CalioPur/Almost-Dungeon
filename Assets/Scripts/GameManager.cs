using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite imgPlayer;
    [SerializeField] private CreateMap createMap;
    [SerializeField] private CardInfo enterDungeonInfo;
    [SerializeField] private SpriteRenderer heroRenderer;
    
    private Transform heroTr;
    
    // Start is called before the first frame update
    void Start()
    {
        createMap.InitMap();
        Vector3 pos = createMap.InitEnterDungeon(enterDungeonInfo);
        heroTr = Instantiate(heroRenderer.gameObject, pos, heroRenderer.transform.rotation).transform;
        heroTr.GetComponent<SpriteRenderer>().sprite = imgPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
