using System.Collections;
using System.Collections.Generic;
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
        TickManager.Instance.SubscribeToMovementEvent(MovementType.Hero, ChangeTick, out entityId);
        Instance = heroes[0];
    }
    
    private void ChangeTick()
    {
        bool isHero0Active = heroes[0].gameObject.activeSelf;
        heroes[0].gameObject.SetActive(!isHero0Active);
        heroes[1].gameObject.SetActive(isHero0Active);
        
        Instance = isHero0Active ? heroes[1] : heroes[0];
    }
}