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
    [SerializeField] private List<string> canNotPlaceHereList;
    [SerializeField] private Vector2Int damagePos;

    private GameObject MinionTokenInstance;
    private bool tutorialIsRunning = false;
    private Vector2Int GoalPos;
    private byte minionMoved = 0;
    private bool ResolveDamage = false;

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
        string text = PlayerPrefs.GetInt("langue",0) == 0 ? "This wandering monster in the dungeon belongs to you. When he sees the hero, he comes to inflict damage on him." : "Ce monstre errant dans le donjon vous appartient. Lorsqu'il voit le héros, il vient lui infliger des dégats.";
        OpenTutorial(DirectionToMove.Up,
            text,
            true, new Vector2Int(-1, -1));
        minionMoved = 1;
    }

    private void Start()
    {
        GameManager.Instance.HeroPosUpdatedEvent += CheckTutorial;
        MinionData.OnOneMinionMoveEvent += MinionMove;
        UI_Dragon.OnDragonTakeDamageEvent += TakeDamageEvent;
        MinionTokenInstance = Instantiate(MinionToken.gameObject, transform);
        MinionTokenInstance.SetActive(false);
        MinionTokenInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // TODO: Remove this line
    }

    private void OnDisable()
    {
        GameManager.Instance.HeroPosUpdatedEvent -= CheckTutorial;
        MinionData.OnOneMinionMoveEvent -= MinionMove;
        UI_Dragon. OnDragonTakeDamageEvent -= TakeDamageEvent;
    }

    public bool CanBePlaced(TileData data)
    {
        MapManager.Instance.GetIndexFromTile(data, out Vector2Int TilePos);
        bool canBePlaced = tutorialDialogs.Exists(x => x.tilePostionGoalPos == TilePos);
        if (ResolveDamage && TilePos == damagePos)
        {
            EndOfTutorial();
            canBePlaced = true;
        }

        else if (!canBePlaced) OpenTutorial(DirectionToMove.Up, canNotPlaceHereList[PlayerPrefs.GetInt("langue",0)], false, new Vector2Int(-1, -1));
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
        }

        return canBePlaced;
    }

    private void TakeDamageEvent()
    {
        DeckManager.Instance.RedrawHand(4);
        ResolveDamage = true;
        string text = PlayerPrefs.GetInt("langue",0) == 0 ? "In front of an exit, the hero will direclty attack you. Place a tile in front of him so he can't reach you !" : "Devant une sortie, le héros t'inflige des degats directement, place plus de tuiles pour éviter qu'il ne t'ateigne !";
        OpenTutorial(DirectionToMove.Up, text,
            true, new Vector2Int(-1, -1));
    }

    private void EndOfTutorial()
    {
        TickManager.Instance.PauseTick(false);
        Destroy(gameObject, 0.4f);
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
            tutorialIsRunning = true;
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

            MinionTokenInstance.transform.DOMove(EndPos.position, 0.5f)
                .SetEase(Ease.InBack);
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
        foreach (var tutorialDialog in tutorialDialogs)
        {
            if (posHero == tutorialDialog.tilePostionToTrigger)
            {
                OpenTutorial(tutorialDialog.direction, tutorialDialog.Dialogs[PlayerPrefs.GetInt("langue",0)], true, tutorialDialog.tilePostionGoalPos);
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