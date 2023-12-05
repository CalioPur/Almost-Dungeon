using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireCamp : MonoBehaviour
{
    private static List<MinionData> MinionDatas = new List<MinionData>();

    private void Awake()
    {
        MinionData.OnMinionDying += StockMinions;
    }

    public void StockMinions(MinionData minion)
    {
        MinionDatas.Add(minion);
    }

    IEnumerator ReviveAnimation()
    {
        TickManager.PauseTick(true);
        yield return new WaitForSeconds(1.0f);
        TickManager.PauseTick(false);
    }
    
    public void Revive()
    {
        StartCoroutine(ReviveAnimation());
        foreach (var minion in MinionDatas)
        {
            minion.Revive();
        }
        MinionDatas.Clear();
    }
}
