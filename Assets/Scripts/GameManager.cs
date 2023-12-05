using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static Action OnGameStartEvent;
    public static Action OnEndDialogEvent;
    public static bool isGameStarted = false;

    [Header("Managers")] [SerializeField] private MapManager mapManager;
    [SerializeField] private TickManager tick;

    [Header("Data")] [SerializeField] private List<HeroesInfo> heroesInfos;
    [SerializeField] private CardInfo enterDungeonInfo;
    [SerializeField] public Vector2Int normsSpawnX;
    [SerializeField] public Vector2Int normsSpawnY;
    public HeroesInfo currentHero;
    public int heroHealthPoint;
    private Vector3 worldPos;
    private Vector2Int startPosHero;
    
    public static GameManager _instance;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        OnGameStartEvent += SpawnHero;
        Time.timeScale = 1;
        mapManager.InitMap();
        mapManager.AddRandomCard();
        mapManager.InitEnterDungeon(enterDungeonInfo.CreateInstance(), normsSpawnX, normsSpawnY, out worldPos, out startPosHero);
        mapManager.CreateFog(startPosHero);
        worldPos += new Vector3(1, 0.1f, 1); //pour que le hero soit au dessus du sol
        OnEndDialogEvent?.Invoke();
    }

    private void OnDisable()
    {
        OnGameStartEvent -= SpawnHero;
        OnGameStartEvent = null;
    }

    private void SpawnHero()
    {
        //int randomHero = Random.Range(0, heroesInfos.Count);
        HeroInstance current = currentHero.CreateInstance();
        current.CurrentHealthPoint = heroHealthPoint;
        Hero heroScript = Instantiate(current.So.prefab, worldPos, current.So.prefab.transform.rotation);
        heroScript.Init(current, startPosHero.x, startPosHero.y, mapManager);
        
        UIManager._instance.heroBlackboard = heroScript.HeroBlackboard;
    }

    // private void CheckIsFirstMove(TileData _, CardHand __, bool canBePlaced)
    // {
    //     if (!canBePlaced) 
    //     {
    //         __.GetImage().transform.position = __.transform.position;
    //         __.removeSelection();
    //         return;
    //     }
    //     MapManager.OnCardTryToPlaceEvent -= CheckIsFirstMove;
    //     SpawnHero();
    //     OnGameStartEvent?.Invoke();
    //     isGameStarted = true;
    // }

    public static void StartGame()
    {
        OnGameStartEvent?.Invoke();
        isGameStarted = true;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}