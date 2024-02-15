using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialDialogData> tutorialDialogs;
    [SerializeField] private List<RectTransform> Positons;
    [SerializeField] private RectTransform TextBox;
    [SerializeField] private TMP_Text Text;

    private void Start()
    {
        GameManager.Instance.HeroPosUpdatedEvent += CheckTutorial;
        PlayerCardController.OnFinishToPose += CloseTutorial;
    }

    private void OnDisable()
    {
        GameManager.Instance.HeroPosUpdatedEvent -= CheckTutorial;
        PlayerCardController.OnFinishToPose -= CloseTutorial;
    }
    
    private void CloseTutorial(CardInfoInstance _)
    {
        TextBox.gameObject.SetActive(false);
        TickManager.Instance.PauseTick(false);
    }

    private void CheckTutorial(Vector2Int posHero)
    {
        if (tutorialDialogs.Count == 0) Destroy(gameObject);
        foreach (var tutorialDialog in tutorialDialogs)
        {
            if (posHero == tutorialDialog.tilePostion)
            {
                TickManager.Instance.PauseTick(true);
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
                        TextBox.position = Positons[0].position;//NOT implemented
                        break;
                }
                Text.text = tutorialDialog.Dialog;
                tutorialDialogs.Remove(tutorialDialog);
                return;
            }
        }
    }
}
