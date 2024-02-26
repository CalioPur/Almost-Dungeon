using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ink.Parsed;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private List<TutorialDialogData> tutorialDialogs;
    [SerializeField] private List<RectTransform> Positons;
    [SerializeField] private RectTransform TextBox;
    [SerializeField] private TMP_Text Text;
    [SerializeField] private Transform MinionToken;
    [SerializeField] private Transform SpawnPos;
    [SerializeField] private Transform EndPos;
    [SerializeField] private CardToBuild[] EnemiesToSpawn;
    [SerializeField] private string canNotPlaceHere;

    private GameObject MinionTokenInstance;
    private bool tutorialIsRunning = false;
    private Vector2Int GoalPos;
    private byte minionMoved = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void MinionMove()
    {
        if (minionMoved > 0) return;
        OpenTutorial(DirectionToMove.Up, "Ce monstre errant dans le donjon vous appartient. Lorsqu'il voit le héros, il vient lui infliger des dégats.", true, new Vector2Int(-1, -1));
        minionMoved = 1;
    }
    
    private void Start()
    {
        GameManager.Instance.HeroPosUpdatedEvent += CheckTutorial;
        MinionData.OnOneMinionMoveEvent += MinionMove;
        MinionTokenInstance = Instantiate(MinionToken.gameObject, transform);
        MinionTokenInstance.SetActive(false);
        MinionTokenInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // TODO: Remove this line
    }

    private void OnDisable()
    {
        GameManager.Instance.HeroPosUpdatedEvent -= CheckTutorial;
        MinionData.OnOneMinionMoveEvent -= MinionMove;
    }

    public bool CanBePlaced(TileData data)
    {
        MapManager.Instance.GetIndexFromTile(data, out Vector2Int TilePos);
        bool canBePlaced = tutorialDialogs.Exists(x => x.tilePostionGoalPos == TilePos);
        if (!canBePlaced) OpenTutorial(DirectionToMove.Up, canNotPlaceHere, false, new Vector2Int(-1, -1));
        else
        {
            
            if (GoalPos == TilePos)
            {
                CloseTutorial(true);
                GoalPos = new Vector2Int(-1, -1);
            }


            for (int i = 0; i < tutorialDialogs.Count; i++)
            {
                if (tutorialDialogs[i].tilePostionGoalPos == TilePos)
                {
                    tutorialDialogs.RemoveAt(i);
                    --i;
                }
            }
            if (tutorialDialogs.Count == 0)
            {
                DeckManager.Instance.RedrawHand(4);
                TickManager.Instance.PauseTick(false);
                Destroy(gameObject);
            }
        }

        return canBePlaced;
    }

    private void CloseTutorial(bool UnPause)
    {
        TextBox.gameObject.SetActive(false);
        tutorialIsRunning = false;
        MinionTokenInstance.transform.DOMove(SpawnPos.position, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            MinionTokenInstance.SetActive(false);
            if (UnPause)
                TickManager.Instance.PauseTick(false);
        });
    }

    private void OpenTutorial(DirectionToMove anchor, string text, bool stopped, Vector2Int GoalposToHit)
    {
        Text.text = text;
        if (GoalposToHit != new Vector2Int(-1, -1))
        {
            GoalPos = GoalposToHit;
        }

        if (!tutorialIsRunning)
        {
            MinionTokenInstance.transform.position = SpawnPos.position;
            MinionTokenInstance.SetActive(true);
            if (stopped)
                TickManager.Instance.PauseTick(true);
            TextBox.gameObject.SetActive(true);
            tutorialIsRunning = true;
            MinionTokenInstance.transform.DOMove(EndPos.position, 0.5f)
                .SetEase(Ease.InBack).OnComplete(() =>
                {
                    TextBox.gameObject.SetActive(true);

                    switch (anchor)
                    {
                        case DirectionToMove.Up:
                            TextBox.position = Positons[0].position;
                            break;
                        case DirectionToMove.Down:
                            TextBox.position = Positons[1].position;
                            break;
                        case DirectionToMove.Left:
                            TextBox.position = Positons[0].position; //NOT implemented
                            break;
                        case DirectionToMove.Right:
                            TextBox.position = Positons[0].position; //NOT implemented
                            break;
                    }
                });
        }
        else
        {
            switch (anchor)
            {
                case DirectionToMove.Up:
                    TextBox.position = Positons[0].position;
                    break;
                case DirectionToMove.Down:
                    TextBox.position = Positons[1].position;
                    break;
                case DirectionToMove.Left:
                    TextBox.position = Positons[0].position; //NOT implemented
                    break;
                case DirectionToMove.Right:
                    TextBox.position = Positons[0].position; //NOT implemented
                    break;
            }

            if (stopped)
                TickManager.Instance.PauseTick(true);
        }
    }

    private void Explose(Vector2Int posHero)
    {
        MapManager.Instance.ChangeTileDataAtPosition(posHero.x, posHero.y);
        DeckManager.Instance.RefreshHand(EnemiesToSpawn);
    }

    private void CheckTutorial(Vector2Int posHero)
    {
        if (tutorialDialogs.Count == 0) Destroy(gameObject);

        foreach (var tutorialDialog in tutorialDialogs)
        {
            if (posHero == tutorialDialog.tilePostionToTrigger)
            {
                OpenTutorial(tutorialDialog.direction, tutorialDialog.Dialog, true, tutorialDialog.tilePostionGoalPos);
                if (tutorialDialog.isExploding)
                {
                    Explose(posHero + new Vector2Int(1, 0));
                }

                return;
            }
        }
    }

    private void Update()
    {
            if (Input.GetMouseButtonDown(0) && minionMoved == 1)
            {
                CloseTutorial(true);
                minionMoved = 2;
            }
    }
}