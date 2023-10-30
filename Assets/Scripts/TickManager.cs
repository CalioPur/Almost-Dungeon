using UnityEngine;
using System;
using System.Collections.Generic;

public enum MovementType
{
    Trap,
    Hero,
    Monster
}

public class TickManager : MonoBehaviour
{
    private static Dictionary<MovementType, List<Action>> movementEvents = new();
    private static Dictionary<MovementType, int> entityIds = new();
    public static event Action OnTick;

    [Range(60, 300)]
    public int BPM = 120;

    private float beatInterval;
    private float nextTickTime;

    void Start()
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

    public static void SubscribeToMovementEvent(MovementType movementType, Action movementAction, int entityId)
    {
        if (!movementEvents.ContainsKey(movementType))
        {
            movementEvents[movementType] = new List<Action>();
        }

        movementEvents[movementType].Add(() => movementAction.Invoke());
        entityIds[movementType] = entityId;
    }

    public static void UnsubscribeFromMovementEvent(MovementType movementType, int entityId)
    {
        if (movementEvents.ContainsKey(movementType))
        {
            // Check if there are any subscribers before unsubscribing
            if (movementEvents[movementType] != null)
            {
                movementEvents[movementType].RemoveAll(action => action?.Target.GetHashCode() == entityId);
            }
        }
        else
        {
            Debug.LogWarning($"No subscribers found for movement type: {movementType}");
        }
    }


    void Tick()
    {
        nextTickTime = Time.time;
        OnTick?.Invoke();

        MovementType currentMovementType = GetMovementTypeFromDivision();

        // Check if the key exists in the dictionary before accessing it
        if (movementEvents.ContainsKey(currentMovementType))
        {
            foreach (Action movementAction in movementEvents[currentMovementType])
            {
                movementAction?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning($"No subscribers found for movement type: {currentMovementType}");
        }
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
}