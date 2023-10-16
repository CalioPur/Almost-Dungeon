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
    
    private GameManager gameManager;

    public enum Direction
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
        Direction direction = ChooseDirection();
        //
        MoveHero(direction);

        StartCoroutine(movementCoroutine(timerUnderMovement));
    }

    private Direction ChooseDirection()
    {
        int index;
        List<float> probabilities = new List<float>();
        foreach (var directions in doorsOpenAndClose)
        {
            int valueOfCellInDirection = directions switch
            {
                Direction.Top => GameManager.Instance.GetValueForExploration(position.x, position.y + 1),
                Direction.Bottom => GameManager.Instance.GetValueForExploration(position.x, position.y - 1),
                Direction.Left => GameManager.Instance.GetValueForExploration(position.x - 1, position.y),
                Direction.Right => GameManager.Instance.GetValueForExploration(position.x + 1, position.y),
                _ => 0
            };
            
            probabilities.Add(100f/(valueOfCellInDirection+1));
        }
        float sum = 0;
        foreach (var probability in probabilities)
        {
            sum += probability;
        }
        for (int i = 0; i < probabilities.Count; i++)
        {
            probabilities[i] = probabilities[i] / sum * 100;
        }

        foreach (var VARIABLE in probabilities)
        {
            //debug for each probability where the hero can go
            Debug.Log("the hero has " + VARIABLE + "% chance to go in this direction " + doorsOpenAndClose[probabilities.IndexOf(VARIABLE)]);
        }
        
        index = Random.Range(0, probabilities.Count);
        
        return doorsOpenAndClose[index];
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
        gameManager = GameManager.Instance;
        StartCoroutine(movementCoroutine(beginMovementDelay));
    }

    public Vector2 GetPosition()
    {
        return new Vector2(position.x, position.y);
    }
}