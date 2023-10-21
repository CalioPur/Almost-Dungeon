using UnityEngine;

public class TickManagerTest : MonoBehaviour
{
    private TickManager tickManager;
    [SerializeField] private GameObject cubeBpm;

    void Start()
    {
        tickManager = GetComponent<TickManager>();
        if (tickManager != null)
        {
            tickManager.OnTick += HandleTick;
            tickManager.BPM = 120f; // Modifiez le BPM au besoin
            tickManager.divisions = 4; // Modifiez le nombre de divisions au besoin
        }
        else
        {
            Debug.LogError("TickManager component not found!");
        }
    }

    void HandleTick(int division)
    {
        Debug.Log($"Tick - Division: {division} - Time: {Time.time}");
        cubeBpm.GetComponent<Renderer>().material.color = division switch
        {
            0 => Color.red,
            1 => Color.blue,
            2 => Color.green,
            3 => Color.yellow,
            _ => cubeBpm.GetComponent<Renderer>().material.color
        };
    }
}   