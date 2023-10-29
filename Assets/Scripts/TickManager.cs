using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static event Action<int> OnTick;
    
    [Range(60, 300)]
    public int BPM = 120;
    [Range(1, 8)]
    public int divisions = 4;

    private float beatInterval;
    private float nextTickTime;

    void Start()
    {
        Initialize(BPM, divisions);
    }

    void Initialize(float bpm, int divisions)
    {
        beatInterval = 60f / bpm;

        if (IsInvoking("Tick"))
        {
            CancelInvoke("Tick");
        }
        InvokeRepeating("Tick", 0f, beatInterval);
    }

    void Tick()
    {
        int currentDivision = (int)((Time.time - nextTickTime + beatInterval) / beatInterval) % divisions;
        OnTick?.Invoke(currentDivision);
        
        if (beatInterval != 60f / BPM)
        {
            CancelInvoke();
            Initialize(BPM, divisions);
        }
    }
}