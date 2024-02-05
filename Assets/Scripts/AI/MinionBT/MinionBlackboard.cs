using UnityEngine;

public class MinionBlackboard : MonoBehaviour
{
    public MinionData minionData;
    [HideInInspector] public DirectionToMove dir = DirectionToMove.None;
    [HideInInspector] public bool firstTimeSeeHero = true;

    public void Reset()
    {
        dir = DirectionToMove.None;
        firstTimeSeeHero = true;
    }
}
