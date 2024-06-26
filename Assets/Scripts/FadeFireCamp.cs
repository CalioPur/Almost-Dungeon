using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeFireCamp : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image blackScreen;

    private void OnEnable()
    {
        FireCamp.OnBeginNightFireCamp += FadeOut;
        FireCamp.OnEndNightFireCamp += FadeIn;
    }

    private void OnDisable()
    {
        FireCamp.OnBeginNightFireCamp -= FadeOut;
        FireCamp.OnEndNightFireCamp -= FadeIn;
    }

    public void FadeIn(float duration)
    {
        canvasGroup.DOFade(0, duration).SetEase(Ease.Linear);
        StartCoroutine(ChangeInteraction(true, 0));
        blackScreen.raycastTarget = false;
    }

    IEnumerator ChangeInteraction(bool interactable, float duration)
    {
        yield return new WaitForSeconds(duration);
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }

    public void FadeOut(float duration)
    {
        canvasGroup.DOFade(1, duration).SetEase(Ease.Linear);
        StartCoroutine(ChangeInteraction(false, duration));
        blackScreen.raycastTarget = true;
    }
}
