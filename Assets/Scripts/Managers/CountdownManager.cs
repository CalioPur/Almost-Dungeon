using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    [Header("params")] [SerializeField] private int maxTick = 3;
    [SerializeField] private float maxSize = 345f;
    [SerializeField] private float minSize = 200f;


    private int currentTick = 0;
    private Coroutine countdownCoroutine;

    private void OnEnable()
    {
        currentTick = maxTick;
        DialogueManager.OnConversationEndedEvent += StartCountdown;
    }

    private void OnDisable()
    {
        DialogueManager.OnConversationEndedEvent -= StartCountdown;
    }

    private void StartCountdown()
    {
        StartCoroutine(UpdateCountdown());
    }

    IEnumerator AnimCountdown()
    {
        countdownText.fontSize = maxSize;
        while (countdownText.fontSize > minSize)
        {
            countdownText.fontSize -= 2f;
            yield return new WaitForSeconds(0.005f);
        }
    }

    private IEnumerator UpdateCountdown()
    {
        yield return new WaitForEndOfFrame();
        while (currentTick > 0)
        {
            yield return new WaitForSeconds(1f);
            if (countdownCoroutine != null)
                StopCoroutine(countdownCoroutine);
            countdownCoroutine = StartCoroutine(AnimCountdown());
            currentTick--;
            countdownText.text = currentTick.ToString();
        }
        gameObject.SetActive(false);
        GameManager.StartGame();
    }
}