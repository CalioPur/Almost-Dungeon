using System.Diagnostics;
using UnityEngine;

public class SwitchHero : Hero
{
    [SerializeField] private Hero[] heroes;

    private int entityIndex;
    private int maxTickToSwitch = 5;
    private int currentTick = 0;

    public override void Init(HeroInstance instance, int _indexHeroX, int _indexHeroY, MapManager manager)
    {
        foreach (var hero in heroes)
        {
            hero.Init(instance, _indexHeroX, _indexHeroY, manager);
            hero.gameObject.SetActive(false);
        }

        entityIndex = 0;
        heroes[entityIndex].gameObject.SetActive(true);
        TickManager.Instance.SubscribeToMovementEvent(MovementType.switchHero, ChangeTick, out entityId);
        GameManager.Instance.HeroInstance = heroes[entityIndex];
        currentTick = maxTickToSwitch;
    }

    private void IncreaseIndex()
    {
        entityIndex++;
        if (entityIndex >= heroes.Length) entityIndex = 0;
    }

    private void SwitchToHero()
    {
        GameManager.Instance.HeroInstance.gameObject.SetActive(false);
        IncreaseIndex();
        GameManager.Instance.HeroInstance = heroes[entityIndex];
        MapManager.Instance.GetWorldPosFromTilePos(GameManager.Instance.HeroInstance.GetIndexHeroPos(),
            out Vector3 worldPos);
        worldPos.y = 0.1f;
        foreach (var hero in heroes)
        {
            hero.transform.position = worldPos;
        }
        GameManager.Instance.HeroInstance.gameObject.SetActive(true);
    }

    private void SwitchRandomPersonnality()
    {
        GameManager.Instance.currentHero.personalities.Clear();
        GameManager.Instance.currentHero.personalities.Add((Personnalities)Random.Range(0, 2));
        GameManager.Instance.currentHero.aggressivity = (Aggressivity)Random.Range(0, 3);
        GameManager.Instance.currentHero.visionType = (VisionType)Random.Range(0, 3);
    }

    private void ChangeTick()
    {
        currentTick--;
        if (currentTick <= 0)
        {
            currentTick = maxTickToSwitch;
            SwitchToHero();
            GameManager.Instance.SwitchLightColor(
                DungeonManager._instance.dungeons[DungeonManager.SelectedBiome].dungeonSO.color[entityIndex]
                );
            SwitchRandomPersonnality();
        }
    }
}