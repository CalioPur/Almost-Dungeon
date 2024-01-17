using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static event Action OnSceneLoadedEvent;
    public static Action OnGameStartEvent;
    public static Action OnEndDialogEvent;
    public static bool isGameStarted = false;

    [Header("Managers")] [SerializeField] private MapManager mapManager;
    [SerializeField] private TickManager tick;

    [Header("Data")] [SerializeField] private List<HeroesInfo> heroesInfos;
    [SerializeField] private List<Light> lightsAmbiant;
    [SerializeField] private CardInfo enterDungeonInfo;
    [SerializeField] public Vector2Int normsSpawnX;
    [SerializeField] public Vector2Int normsSpawnY;
    public HeroSO currentHero;
    public int heroHealthPoint;
    public int heroCurrentHealthPoint;
    public int rotationOfSpawnTile;
    private Vector3 worldPos;
    private Vector2Int startPosHero;

    //public int dragonHealthPoint = 10;
    
    public static GameManager _instance;
    
    private void Awake()
    {
        
        OnSceneLoadedEvent?.Invoke();
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        //DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        OnGameStartEvent += SpawnHero;
        //Time.timeScale = 1;
        mapManager.InitMap();
        mapManager.AddRandomCard();
        mapManager.InitEnterDungeon(enterDungeonInfo.CreateInstance(), normsSpawnX, normsSpawnY,rotationOfSpawnTile, out worldPos, out startPosHero);
        mapManager.CreateFog(startPosHero);
        worldPos += new Vector3(1, 0.1f, 1); //pour que le hero soit au dessus du sol
        OnEndDialogEvent?.Invoke();
        foreach (var light in lightsAmbiant)
        {
            light.color = DungeonManager._instance.dungeons[DungeonManager.SelectedBiome].dungeonSO.color;
        }
    }

    private void OnDisable()
    {
        OnGameStartEvent -= SpawnHero;
        OnGameStartEvent = null;
    }

    private void SpawnHero()
    {
        //int randomHero = Random.Range(0, heroesInfos.Count);
        if (currentHero == null)
        {
            Debug.LogError("No hero selected");
            return;
        }
        HeroInstance current = currentHero.classe.CreateInstance();
        current.CurrentHealthPoint = heroHealthPoint;
        
        Hero heroScript = Instantiate(current.So.prefab, worldPos, current.So.prefab.transform.rotation);
        heroScript.Init(current, startPosHero.x, startPosHero.y, mapManager);
        heroScript.HeroBlackboard.visionType = currentHero.visionType;
        // heroScript.HeroBlackboard.aggressivity = currentHero.aggressivity;
        
        if (heroCurrentHealthPoint < heroHealthPoint) //Degats pris pendant le dialogue
        {
            heroScript.TakeDamage((heroHealthPoint - heroCurrentHealthPoint), AttackType.Fire);
        }
        
        UIManager._instance.heroBlackboard = heroScript.HeroBlackboard;
    }

    public static void StartGame()
    {
        OnGameStartEvent?.Invoke();
        isGameStarted = true;
    }
    public void RestartGame()
    {
        
        FindObjectOfType<DungeonManager>().LoadNextLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}