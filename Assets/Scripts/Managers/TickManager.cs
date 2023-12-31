using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public enum MovementType
{
    Trap,
    Hero,
    Monster
}

public struct TickData
{
    public int ID;
    public Action _action;
}

public class TickManager : MonoBehaviour
{
    private static Dictionary<MovementType, List<TickData>> movementEvents = new();
    private static Dictionary<MovementType, int> entityIds = new();
    public static event Action OnTick;

    [Range(0f, 1000f)]
    public int BPM = 120;

    [Range(0.1f, 5f)] 
    public float actionsTime; //this is the time for all of the actions to be completed

    private float beatInterval;
    private float nextTickTime;
    private static bool TickOnPaused = false;
    private static int index = 0;

    void Awake()
    {
        GameManager.OnEndDialogEvent += LaunchBPM;
    }

    private void OnDisable()
    {
        GameManager.OnEndDialogEvent -= LaunchBPM;
        index = 0;
    }
    
    void LaunchBPM()
    {
        Initialize(BPM);
    }
    
    void Initialize(int bpm)
    {
        beatInterval = 60f / bpm;
        if (IsInvoking("Tick"))
        {
            CancelInvoke("Tick");
        }
        InvokeRepeating("Tick", 0f, beatInterval);
    }

    public static void SubscribeToMovementEvent(MovementType movementType, Action movementAction, out int entityId)
    {
        if (!movementEvents.ContainsKey(movementType))
        {
            movementEvents[movementType] = new List<TickData>();
        }

        entityId = ++index;
        TickData newData = new TickData();
        newData.ID = entityId;
        newData._action = () => movementAction.Invoke();
        movementEvents[movementType].Add(newData);
        entityIds[movementType] = entityId;
    }

    public static void UnsubscribeFromMovementEvent(MovementType movementType, int entityId)
    {
        if (movementEvents.ContainsKey(movementType))
        {
            // Check if there are any subscribers before unsubscribing
            if (movementEvents[movementType] != null)
            {
                movementEvents[movementType].Remove(movementEvents[movementType].Find(tickData => tickData.ID == entityId));
            }
        }
        else
        {
            //Debug.LogWarning($"No subscribers found for movement type: {movementType}");
        }
    }


    void Tick()
    {
        if (TickOnPaused) return;
        nextTickTime = Time.time;
        OnTick?.Invoke();

        MovementType currentMovementType = GetMovementTypeFromDivision();

        // Check if the key exists in the dictionary before accessing it
        if (movementEvents.TryGetValue(currentMovementType, out var datas))
        {
            // Check if there are any subscribers before invoking
            if (datas != null)
            {
                datas.ForEach(tickData => tickData._action?.Invoke());
            }
        }
    }

    public static void PauseTick(bool pause)
    {
        TickOnPaused = pause;
    }


    int tickCount = 0;

    MovementType GetMovementTypeFromDivision()
    {
        tickCount++;

        switch (tickCount % 3)
        {
            case 1:
                return MovementType.Trap;
            case 2:
                return MovementType.Hero;
            case 0:
                return MovementType.Monster;
            default:
                throw new InvalidOperationException("Invalid tick count.");
        }
    }
    
    //while in the inspector do that
    private void OnValidate()
    {
        //change the BPM when the slider for actions time is changed so the bpm is equivalent to the actions time for exemple we have 3 type of actions and the actions time is 3s so each actions take 1s so the bpm is 60
        BPM = Mathf.RoundToInt(60 / actionsTime );
    }
    
    
}