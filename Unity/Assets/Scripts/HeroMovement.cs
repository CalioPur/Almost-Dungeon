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
    
    [SerializeField] private TMP_Text probaTop;
    [SerializeField] private TMP_Text probaBottom;
    [SerializeField] private TMP_Text probaLeft;
    [SerializeField] private TMP_Text probaRight;

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


        DoorsAtHeroPos = GameManager.Instance.GetDoorAtPosition(position);
        doorsOpenAndClose = new List<Direction>();

        FoundDoor();

        // A Changer car pour l'instant deplacement aleatoire
        Direction direction = ChooseDirection();
        //
        MoveHero(direction);
        int numberOfEnemies = GameManager.Instance.GetNumberOfEnemiesAtPosition(position);
        if (numberOfEnemies > 0)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                yield return new WaitForSeconds(timerUnderMovement/4);
                HeroStatus.Instance.LooseHp(1);
                GameManager.Instance.DecreaseNumberOfEnemiesAtPosition(position);
            }
        }
        StartCoroutine(movementCoroutine(timerUnderMovement));
    }

    private Direction ChooseDirection()
    {
        int index;
        int weight = 5;
        List<float> probabilities = new List<float>();
        float proba;
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
            proba = 100f / (valueOfCellInDirection*weight + 1);
            probabilities.Add(proba);
        }
        float sum = 0;
        foreach (var probability in probabilities)
        {
            sum += probability;
        }
        for (int i = 0; i < probabilities.Count; i++)
        {
            probabilities[i] = probabilities[i] / sum * 100;
            Debug.Log("proba : " + probabilities[i]);
        }
        bool changedTop = false;
        bool changedBottom = false;
        bool changedLeft = false;
        bool changedRight = false;
        foreach (var VARIABLE in probabilities)
        {
            switch (doorsOpenAndClose[probabilities.IndexOf(VARIABLE)])
            {
                case Direction.Top:
                    // probaTop.text = "Top : " + VARIABLE + "%";
                    probaTop.text = "\u2191 : " + Math.Round(VARIABLE, 2) + "%";
                    changedTop = true;
                    break;
                case Direction.Bottom:
                    // probaBottom.text = "Bottom : " + VARIABLE + "%";
                    probaBottom.text = "\u2193 : " + Math.Round(VARIABLE, 2) + "%";
                    changedBottom = true;
                    break;
                case Direction.Left:
                    // probaLeft.text = "Left : " + VARIABLE + "%";
                    probaLeft.text = "\u2190 : " + Math.Round(VARIABLE, 2) + "%";
                    changedLeft = true;
                    break;
                case Direction.Right:
                    // probaRight.text = "Right : " + VARIABLE + "%";
                    probaRight.text = "\u2192 : " + Math.Round(VARIABLE, 2) + "%";
                    changedRight = true;
                    break;
                default:
                    break;
            }
        }
        if (!changedTop) probaTop.text = "\u2191 : 0%";
        if (!changedBottom) probaBottom.text = "\u2193 : 0%";
        if (!changedLeft) probaLeft.text = "\u2190 : 0%";
        if (!changedRight) probaRight.text = "\u2192 : 0%";
        
        //if a probability is > 70% then we take it
        for (int i = 0; i < probabilities.Count; i++)
        {
            if (probabilities[i] > 70)
            {
                Debug.Log("Direction : " + doorsOpenAndClose[i] + "this move had a probability of " + probabilities[i] + "%");
                return doorsOpenAndClose[i];
            }
        }
        
        float random = Random.Range(0, 100);
        float sumProba = 0;
        index = 0;
        foreach (var probability in probabilities)
        {
            sumProba += probability;
            if (random < sumProba)
            {
                break;
            }
            index++;
        }
        
        Debug.Log("Direction : " + doorsOpenAndClose[index] + "this move had a probability of " + probabilities[index] + "%");
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