using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireCamp : MonoBehaviour
{
    public static event Action<float> OnBeginNightFireCamp;
    public static event Action<float> OnEndNightFireCamp;
    
    private static List<MinionData> MinionDatas = new ();

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
        OnBeginNightFireCamp?.Invoke(1f);
        yield return new WaitForSeconds(2f);
        foreach (var minion in MinionDatas)
        {
            minion.Revive();
        }
        MinionDatas.Clear();
        OnEndNightFireCamp?.Invoke(1);
        yield return new WaitForSeconds(2.0f);
        TickManager.PauseTick(false);
    }
    
    public void Revive()
    {
        StartCoroutine(ReviveAnimation());

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Revive();
        }
    }
}
