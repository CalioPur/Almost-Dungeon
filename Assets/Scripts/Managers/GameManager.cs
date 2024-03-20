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
    public event Action<Vector2Int> HeroPosUpdatedEvent;

    [Header("Managers")] [SerializeField] private MapManager mapManager;
    [SerializeField] private TutorialManager tutorialManagerPrefab;
    [SerializeField] private TickManager tick;

    [Header("Data")] [SerializeField] private List<HeroesInfo> heroesInfos;
    [SerializeField] private List<Light> lightsAmbiant;
    [SerializeField] private CardInfo enterDungeonInfo;
    public Transform AttackPoint;
    public bool EndOfGame = false;
    public HeroSOInstance currentHero;
    public int heroHealthPoint;
    public int heroCurrentHealthPoint;
    public int rotationOfSpawnTile;
    public bool isInDialogue = false;
    public static GameManager Instance;
    public HeroInstance current { get; private set; }
    public Hero HeroInstance { get; set; }

    private Vector3 worldPos;
    private Vector2Int posHero;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        OnSceneLoadedEvent?.Invoke();
    }

    public void SwitchLightColor(Color color)
    {
        foreach (var light in lightsAmbiant)
        {
            light.color = color;
        }
    }

    void Start()
    {
        //OnGameStartEvent += SpawnHero();
        //Time.timeScale = 1;
        mapManager.InitMap();
        mapManager.InitEnterDungeon(enterDungeonInfo.CreateInstance(), rotationOfSpawnTile, out worldPos, posHero);
        worldPos += new Vector3(0, 0.1f, 0);

        SwitchLightColor(DungeonManager._instance.dungeons[DungeonManager.SelectedBiome].dungeonSO.color[0]);

        if (!DungeonManager._instance.TutorialDone)
            Instantiate(tutorialManagerPrefab);
        SpawnHero();
    }

    private void OnDisable()
    {
        OnGameStartEvent -= SpawnHero;
        OnGameStartEvent = null;
    }

    public void SpawnHero()
    {
        //int randomHero = Random.Range(0, heroesInfos.Count);
        if (currentHero == null)
        {
            Debug.LogError("No hero selected");
            return;
        }

        current = currentHero.classe.CreateInstance();
        current.CurrentHealthPoint = heroHealthPoint;

        HeroInstance = Instantiate(current.So.prefab, worldPos, current.So.prefab.transform.rotation);
        HeroInstance.Init(current, posHero.x, posHero.y, mapManager);

        if (heroCurrentHealthPoint < heroHealthPoint) //Degats pris pendant le dialogue
        {
            HeroInstance.TakeDamage((heroHealthPoint - heroCurrentHealthPoint), AttackType.Fire);
        }

        UIManager._instance.heroBlackboard = HeroInstance.HeroBlackboard;
        OnEndDialogEvent?.Invoke();
    }

    public static void StartGame()
    {
        OnGameStartEvent?.Invoke();
        TickManager.Instance.PauseTick(false);
        isGameStarted = true;
    }

    public void UpdateHeroPos(Vector2Int getIndexHeroPos)
    {
        posHero = getIndexHeroPos;
        HeroPosUpdatedEvent?.Invoke(posHero);
    }

    public Vector2Int GetHeroPos()
    {
        return posHero;
    }

    public float GetHeroSpeed()
    {
        if (currentHero == null) return -1;
        return currentHero.speed;
    }
}