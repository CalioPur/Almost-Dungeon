using UnityEngine;

public class SwitchHero : Hero
{
    [SerializeField] private Hero[] heroes;

    public override void Init(HeroInstance instance, int _indexHeroX, int _indexHeroY, MapManager manager)
    {
        foreach (var hero in heroes)
        {
            hero.Init(instance, _indexHeroX, _indexHeroY, manager);
        }
        heroes[0].gameObject.SetActive(true);
        heroes[1].gameObject.SetActive(false);
        TickManager.Instance.SubscribeToMovementEvent(MovementType.switchHero, ChangeTick, out entityId);
        Instance = heroes[0];
    }
    
    private void ChangeTick()
    {
        bool isHero0Active = heroes[0].gameObject.activeSelf;
        Instance = isHero0Active? heroes[1] : heroes[0];
        MapManager.Instance.GetWorldPosFromTilePos(Instance.GetIndexHeroPos(), out Vector3 worldPos);
        worldPos.y = 0.1f;
        foreach (var hero in heroes)
        {
            hero.transform.position = worldPos;
        }
        heroes[0].gameObject.SetActive(!isHero0Active);
        heroes[1].gameObject.SetActive(isHero0Active);
        
    }
}