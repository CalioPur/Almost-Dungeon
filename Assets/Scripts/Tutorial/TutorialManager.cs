using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ink.Parsed;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialDialogData> tutorialDialogs;
    [SerializeField] private List<RectTransform> Positons;
    [SerializeField] private RectTransform TextBox;
    [SerializeField] private TMP_Text Text;
    [SerializeField] private Transform MinionToken;
    [SerializeField] private Transform SpawnPos;
    [SerializeField] private Transform EndPos;
    [SerializeField] private CardToBuild[] EnemiesToSpawn;

    private GameObject MinionTokenInstance;

    private void Start()
    {
        GameManager.Instance.HeroPosUpdatedEvent += CheckTutorial;
        PlayerCardController.OnFinishToPose += CloseTutorial;
        MinionTokenInstance = Instantiate(MinionToken.gameObject, transform);
        MinionTokenInstance.SetActive(false);
        MinionTokenInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // TODO: Remove this line
    }

    private void OnDisable()
    {
        GameManager.Instance.HeroPosUpdatedEvent -= CheckTutorial;
        PlayerCardController.OnFinishToPose -= CloseTutorial;
    }

    private void CloseTutorial(CardInfoInstance _)
    {
        TextBox.gameObject.SetActive(false);
        MinionTokenInstance.transform.DOMove(SpawnPos.position, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            MinionTokenInstance.SetActive(false);
            TickManager.Instance.PauseTick(false);
        });
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
            if (posHero == tutorialDialog.tilePostion)
            {
                MinionTokenInstance.transform.position = SpawnPos.position;
                MinionTokenInstance.SetActive(true);
                TickManager.Instance.PauseTick(true);
                if (tutorialDialog.isExploding)
                {
                    Explose(posHero + new Vector2Int(1, 0));
                }
                MinionTokenInstance.transform.DOMove(EndPos.position, 0.5f)
                    .SetEase(Ease.InBack).OnComplete(() =>
                    {
                        TextBox.gameObject.SetActive(true);

                        switch (tutorialDialog.direction)
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

                        Text.text = tutorialDialog.Dialog;
                        tutorialDialogs.Remove(tutorialDialog);
                    });

                return;
            }
        }
    }
}