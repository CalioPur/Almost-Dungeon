using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
   [SerializeField] private TMP_Text countdownText;
   
   [Header("params")]
   [SerializeField] private int maxTick = 3;
   [SerializeField] private float maxSize = 345f;
   [SerializeField] private float minSize = 200f;
   
   
   private int currentTick = 0;
   private Coroutine countdownCoroutine;

   IEnumerator CD()
   {
      yield return new WaitForEndOfFrame();
      TickManager.Instance.OnTick += UpdateCountdown;
   }
   
   private void OnEnable()
   {
      currentTick = maxTick;
      StartCoroutine(CD());
   }

   private void OnDisable()
   {
      TickManager.Instance.OnTick -= UpdateCountdown;
   }
   
   IEnumerator AnimCountdown()
   {
      countdownText.fontSize = maxSize;
      while (countdownText.fontSize > minSize)
      {
         countdownText.fontSize -= 1f;
         yield return new WaitForSeconds(0.005f);
      }
   }
   
   private void UpdateCountdown()
   {
      if (countdownCoroutine != null)
         StopCoroutine(countdownCoroutine);
      countdownCoroutine = StartCoroutine(AnimCountdown());
      currentTick--;
      countdownText.text = currentTick.ToString();
      if (currentTick <= 0)
      {
         gameObject.SetActive(false);
         GameManager.StartGame();
      }
   }
}
