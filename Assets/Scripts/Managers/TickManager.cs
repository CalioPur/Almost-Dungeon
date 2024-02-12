using UnityEngine;
using System;
using System.Collections;
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
    public static event Action OnHeroTick;
    public static event Action OnMinionTick;
    public static event Action OnTrapTick;
    public static TickManager Instance;

    [HideInInspector][Range(0f, 1000f)] public int BPM = 120;

    [HideInInspector][Range(0.1f, 5f)] public float actionsTime; //this is the time for all of the actions to be completed

    [SerializeField] private AnimationCurve BPMBoostCurve;

    private float beatInterval;
    private float nextTickTime;
    private static bool TickOnPaused = false;
    private static int index = 0;
    private static bool EndGame = false;
    private float elapsedTime = 0f;
    private Coroutine bpmCoroutine;

    void Awake()
    {
        GameManager.OnEndDialogEvent += LaunchBPM;
        EndGame = false;
        Instance = this;
        
    }

    private void OnDisable()
    {
        GameManager.OnEndDialogEvent -= LaunchBPM;
        index = 0;
        movementEvents = new();
        entityIds = new();
        EndGame = false;
        StopCoroutine(bpmCoroutine);
    }

    void LaunchBPM()
    {
        Initialize(BPM);
    }

    public float calculateBPM()
    {
        float bpm = beatInterval;
        float speed = GameManager.Instance.GetHeroSpeed();
        if (speed > 0)
        {
            if (elapsedTime > BPMBoostCurve[BPMBoostCurve.length - 1].time)
                speed += BPMBoostCurve[BPMBoostCurve.length - 1].value;
            else
                speed += BPMBoostCurve.Evaluate(elapsedTime);
            bpm = 1.0f / speed;
        }

        return bpm;
    }

    IEnumerator BPMLauncher()
    {
        float time = 0;
        yield return new WaitForSeconds(3);
        while (true)
        {
            Tick();
            time = calculateBPM();
            yield return new WaitForSeconds(calculateBPM());
            elapsedTime += time  / 60.0f;
        }
    }

    void Initialize(int bpm)
    {
        beatInterval = 1.0f;
        elapsedTime = 0f;
        // if (IsInvoking("Tick"))
        // {
        //     CancelInvoke("Tick");
        // }
        //
        // InvokeRepeating("Tick", 0f, beatInterval);
        bpmCoroutine = StartCoroutine(BPMLauncher());
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
                movementEvents[movementType]
                    .Remove(movementEvents[movementType].Find(tickData => tickData.ID == entityId));
            }
        }
    }


    void Tick()
    {
        if (TickOnPaused || EndGame) return;

        nextTickTime = Time.time;
        OnTick?.Invoke();

        MovementType currentMovementType = GetMovementTypeFromDivision();

        // Check if the key exists in the dictionary before accessing it
        if (movementEvents.TryGetValue(currentMovementType, out var datas))
        {
            // Check if there are any subscribers before invoking
            if (datas != null)
            {
                //datas.ForEach(tickData => tickData._action?.Invoke());
                for (int i = 0; i < datas.Count; i++)
                {
                    if (i < datas.Count && datas[i]._action != null)
                        datas[i]._action.Invoke();
                }
            }
        }
    }

    // private int DistanceFromClosestExit()
    // {
    //     return ALaid1.distanceToExit;
    // }

    public static void PauseTick(bool pause)
    {
        TickOnPaused = pause;
    }


    byte tickCount = 0;

    MovementType GetMovementTypeFromDivision()
    {
        tickCount++;
        tickCount %= 4;

        switch (tickCount)
        {
            case 0:
                OnMinionTick?.Invoke();
                return MovementType.Monster;
            case 1:
                OnTrapTick?.Invoke();
                return MovementType.Trap;
            case 2:
                OnHeroTick?.Invoke();
                return MovementType.Hero;
            case 3:
                return MovementType.Trap;
            default:
                throw new InvalidOperationException("Invalid tick count.");
        }
    }

    //while in the inspector do that
    private void OnValidate()
    {
        //change the BPM when the slider for actions time is changed so the bpm is equivalent to the actions time for exemple we have 3 type of actions and the actions time is 3s so each actions take 1s so the bpm is 60
        BPM = Mathf.RoundToInt(90 / actionsTime);
    }

    public static void OnEndGame()
    {
        EndGame = true;
    }
}