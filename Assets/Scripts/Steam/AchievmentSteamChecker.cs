using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AchievmentSteamChecker : MonoBehaviour
{
    public static AchievmentSteamChecker _instance;
    
    private bool damage = false;
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
        AttackMinion.HeroTurnStartEvent += ResetDamageInflictedThisTurn;
        AttackHero.DealDamageEvent += AddDamageInflictedThisTurn;
        AttackHero.UnlockSniperAchievementEvent += UnlockSniperAchievement;
        minionSkeleton.OnResurectEvent += UnlockResurectAchievement;
        
    }

    

    public void UnlockEndLevelAchievment(int biomeIndex)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement("DUNGEON_" + biomeIndex);
        SteamUserStats.StoreStats();
        UnlockNoDamageAchievment();
    }
    
    private void UnlockNoDamageAchievment()
    {
        if (!damage)
        {
            if (!SteamManager.Initialized) return;
            SteamUserStats.SetAchievement("UNTOUCHABLE");
            SteamUserStats.StoreStats();
        }
        damage = false; //cette fonction est appelée à chaque fin de niveau, donc on reset le damage
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
    
    private void DragonTakeDamage()
    {
        damage = true;
    }
    
    public void AddDamageInflictedThisTurn(int dmg)
    {
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
        AttackMinion.HeroTurnStartEvent -= ResetDamageInflictedThisTurn;
        AttackHero.DealDamageEvent -= AddDamageInflictedThisTurn;
    }
}
