using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialDialogData> tutorialDialogs;
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
                Text.text = tutorialDialog.Dialog;
                tutorialDialogs.Remove(tutorialDialog);
                return;
            }
        }
    }
}
