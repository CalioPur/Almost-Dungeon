using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;



public class HeroMovement : MonoBehaviour
{
    public static HeroMovement Instance;

    [SerializeField] private float beginMovementDelay;
    [SerializeField] private float timerUnderMovement;
    [SerializeField] private TMP_Text text;

    private Vector2Int position;
    private List<bool> DoorsAtHeroPos;
    private List<Direction> doorsOpenAndClose;
    
    private enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
    }
    
    private void FoundDoor()
    {
        if (DoorsAtHeroPos[0]) doorsOpenAndClose.Add(Direction.Top);
        if (DoorsAtHeroPos[1]) doorsOpenAndClose.Add(Direction.Bottom);
        if (DoorsAtHeroPos[2]) doorsOpenAndClose.Add(Direction.Left);
        if (DoorsAtHeroPos[3]) doorsOpenAndClose.Add(Direction.Right);
    }

    private IEnumerator movementCoroutine(float timer)
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            text.text = "Hero moves on " + timer.ToString("0.00");
            yield return null;
        }

        Debug.Log("Movement begin");

        DoorsAtHeroPos = GameManager.Instance.GetDoorAtPosition(position);
        doorsOpenAndClose = new List<Direction>();

        FoundDoor();

        // A Changer car pour l'instant deplacement aleatoire
        Direction direction = doorsOpenAndClose[Random.Range(0, doorsOpenAndClose.Count)];
        //
        MoveHero(direction);

        StartCoroutine(movementCoroutine(timerUnderMovement));
    }

    private void MoveHero(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
                position.y++;
                break;
            case Direction.Bottom:
                position.y--;
                break;
            case Direction.Left:
                position.x--;
                break;
            case Direction.Right:
                position.x++;
                break;
        }
        GameManager.Instance.SetHeroPosition(position);
    }
    
    public void SetPosition(Vector2Int pos)
    {
        position = pos;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(movementCoroutine(beginMovementDelay));
    }
}