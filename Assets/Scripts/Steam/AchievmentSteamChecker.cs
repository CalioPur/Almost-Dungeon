using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AchievmentSteamChecker : MonoBehaviour
{
    public static AchievmentSteamChecker _instance;
    private void Start()
    {
        if (!SteamManager.Initialized) return;
        print("SteamManager has initialized");
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    
    public void UnlockEndLevelAchievment(int biomeIndex)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement("DUNGEON_" + biomeIndex);
        SteamUserStats.StoreStats();
    }
}
