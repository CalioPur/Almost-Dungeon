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
    public HeroSO currentHero;
    public int heroHealthPoint;
    public int heroCurrentHealthPoint;
    public int rotationOfSpawnTile;
    public bool isInDialogue = false;
    private Vector3 worldPos;
    private Vector2Int posHero;
    public static GameManager Instance;

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

    void Start()
    {
        OnGameStartEvent += SpawnHero;
        mapManager.InitMap();
        mapManager.AddRandomCard();
        mapManager.InitEnterDungeon(enterDungeonInfo.CreateInstance(), rotationOfSpawnTile, out worldPos, posHero);
        worldPos += new Vector3(0, 0.1f, 0);

        foreach (var light in lightsAmbiant)
        {
            light.color = DungeonManager._instance.dungeons[DungeonManager.SelectedBiome].dungeonSO.color;
        }

        if (!DungeonManager._instance.TutorialDone)
            Instantiate(tutorialManagerPrefab);
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
        heroScript.Init(current, posHero.x, posHero.y, mapManager);
        heroScript.HeroBlackboard.visionType = currentHero.visionType;
        heroScript.HeroBlackboard.aggressivity = currentHero.aggressivity;
        heroScript.HeroBlackboard.personalities = currentHero.personalities;

        if (heroCurrentHealthPoint < heroHealthPoint) //Degats pris pendant le dialogue
        {
            heroScript.TakeDamage((heroHealthPoint - heroCurrentHealthPoint), AttackType.Fire);
        }

        UIManager._instance.heroBlackboard = heroScript.HeroBlackboard;
        OnEndDialogEvent?.Invoke();
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
        if (!currentHero) return -1;
        return currentHero.speed;
    }
}