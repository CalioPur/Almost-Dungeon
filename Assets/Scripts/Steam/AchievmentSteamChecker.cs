using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AchievmentSteamChecker : MonoBehaviour
{
    public static AchievmentSteamChecker _instance;
    
    private bool damaged = false;
    private bool heroDamagedByMinion = false;
    private int damageInflictedThisTurn = 0;
    
    private void Start()
    {
        
        print("SteamManager has initialized");
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        if (!SteamManager.Initialized) return;
        UI_Dragon.OnDragonTakeDamageEvent += DragonTakeDamage;
        UI_Hero.OnEndGameEvent += UnlockComeFightMeAchievement;
        AttackMinion.HeroTurnStartEvent += ResetDamageInflictedThisTurn;
        AttackHero.DealDamageEvent += AddDamageInflictedThisTurn;
        Pyke.DealDamageEvent += AddDamageInflictedThisTurn;
        AttackHero.UnlockSniperAchievementEvent += UnlockSniperAchievement;
        minionSkeleton.OnResurectEvent += UnlockResurectAchievement;
        DungeonManager.OnLevelLoaded += NewLevel;
        
    }

    

    public void UnlockEndLevelAchievment(int biomeIndex)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement("DUNGEON_" + biomeIndex);
        SteamUserStats.StoreStats();
        UnlockNoDamageAchievment();
        
    }
    
    public void UnlockMachAchievment(int machIndex)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement("MACH_" + machIndex);
        SteamUserStats.StoreStats();
    }
    

    private void UnlockNoDamageAchievment()
    {
        if (!damaged)
        {
            if (!SteamManager.Initialized) return;
            SteamUserStats.SetAchievement("UNTOUCHABLE");
            SteamUserStats.StoreStats();
        }
        damaged = false; //cette fonction est appelée à chaque fin de niveau, donc on reset le damage
    }
    
    private void UnlockSniperAchievement()
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement("SNIPER");
        SteamUserStats.StoreStats();
    }
    private void UnlockResurectAchievement(int resurectCount)
    {
        if (!SteamManager.Initialized) return;
        if (resurectCount >= 2)
        {
            SteamUserStats.SetAchievement("NECROMANCER");
            SteamUserStats.StoreStats();
        }
    }
    
    public void UnlockAchievementFromDialogue(string s)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement(s);
        SteamUserStats.StoreStats();
    }
    
    private void UnlockComeFightMeAchievement(bool won)
    {
        if (!heroDamagedByMinion && won)
        {
            if (!SteamManager.Initialized) return;
            SteamUserStats.SetAchievement("COME_FIGHT");
            SteamUserStats.StoreStats();
        }
    }

    private void NewLevel()
    {
        heroDamagedByMinion = false;
    }
    
    private void DragonTakeDamage()
    {
        damaged = true;
    }
    
    public void AddDamageInflictedThisTurn(int dmg)
    {
        heroDamagedByMinion = true;
        damageInflictedThisTurn += dmg;
        if (damageInflictedThisTurn >= 7)
        {
            if (!SteamManager.Initialized) return;
            SteamUserStats.SetAchievement("AMBUSH");
            SteamUserStats.StoreStats();
        }
    }
    
    private void ResetDamageInflictedThisTurn()
    {
        damageInflictedThisTurn = 0;
    }
    
    private void OnDestroy()
    {
        UI_Dragon.OnDragonTakeDamageEvent -= DragonTakeDamage;
        UI_Hero.OnEndGameEvent -= UnlockComeFightMeAchievement;
        AttackMinion.HeroTurnStartEvent -= ResetDamageInflictedThisTurn;
        AttackHero.DealDamageEvent -= AddDamageInflictedThisTurn;
        Pyke.DealDamageEvent -= AddDamageInflictedThisTurn;
        AttackHero.UnlockSniperAchievementEvent -= UnlockSniperAchievement;
        minionSkeleton.OnResurectEvent -= UnlockResurectAchievement;
        DungeonManager.OnLevelLoaded -= NewLevel;
    }

    
}
