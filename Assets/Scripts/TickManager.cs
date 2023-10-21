using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    [Range(60, 300)]
    public int BPM = 120;
    [Range(1, 8)]
    public int divisions = 4;

    private float beatInterval;
    private float nextTickTime;
    public event Action<int> OnTick;

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
        
        if (beatInterval != 60f / BPM || divisions != divisions)
        {
            CancelInvoke();
            Initialize(BPM, divisions);
        }
    }
}