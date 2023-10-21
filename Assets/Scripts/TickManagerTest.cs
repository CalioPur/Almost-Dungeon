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
        //change the color of the cube for each division
        switch (division)
        {
            case 0:
                cubeBpm.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 1:
                cubeBpm.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case 2:
                cubeBpm.GetComponent<Renderer>().material.color = Color.green;
                break;
            case 3:
                cubeBpm.GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }
}   