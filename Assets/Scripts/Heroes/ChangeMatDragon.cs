using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChangeMatDragon : MonoBehaviour
{
       [SerializeField] private Material dragonMatTrans;
       [SerializeField] private Material dragonMatOpaque;
       [SerializeField] private Material dragonMatHit;
       [SerializeField] private MeshRenderer dragonMesh;
       
       [SerializeField] private Material dragonMatEyeTrans;
       [SerializeField] private Material dragonMatEyeOpaque;
       [SerializeField] private List<MeshRenderer> dragonEyeMesh;
       [SerializeField] private float alphaBasisValue = 1.0f;


       private IEnumerator DisappearAfterDelay(float delay)
       {
              float remainingTime = delay;
              while (remainingTime > 0)
              {
                     remainingTime -= Time.deltaTime;
                     dragonMesh.material.color = new Color(1, 1, 1, alphaBasisValue * (remainingTime / delay));
                     yield return null;
              }


              gameObject.SetActive(false);
       }
       
       private void RenderTransparent(float delay)
       {
              dragonEyeMesh.ForEach(x => x.material = dragonMatEyeTrans);
              dragonMesh.material = dragonMatTrans;
              dragonMesh.material.color = new Color(1, 1, 1, alphaBasisValue);
              StartCoroutine(DisappearAfterDelay(delay));
       }

       private IEnumerator RenderTransAfterDelay(float delay, float delayBeforeDisapear)
       {
              yield return new WaitForSeconds(delay);
              RenderTransparent(delayBeforeDisapear);
              transform.DOKill();
       }
       
       public void TakeHit(float delayBeforeDisapear)
       {
              dragonEyeMesh.ForEach(x => x.material = dragonMatEyeOpaque);
              dragonMesh.material = dragonMatHit;
              StartCoroutine(RenderTransAfterDelay(0.05f, delayBeforeDisapear));
       }
}
