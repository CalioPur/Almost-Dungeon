using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AchievmentTest : MonoBehaviour
{
    private void Start()
    {
        if(!SteamManager.Initialized) return;
        
        SteamUserStats.SetAchievement("TEST_SUCCES");
        SteamUserStats.StoreStats();
        
        SteamUserStats.GetAchievement("TEST_SUCCES", out var achieved);
        Debug.Log("TEST_SUCCES : " + achieved);
    }
}
