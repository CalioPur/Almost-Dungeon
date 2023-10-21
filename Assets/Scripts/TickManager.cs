using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public float BPM = 120f;
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
        nextTickTime = Time.time + beatInterval;
        InvokeRepeating("Tick", 0f, beatInterval);
    }

    void Tick()
    {
        int currentDivision = (int)((Time.time - nextTickTime + beatInterval) / beatInterval) % divisions;
        OnTick?.Invoke(currentDivision);
    }
}