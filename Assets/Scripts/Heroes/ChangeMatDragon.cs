using System;
using System.Collections;
using System.Collections.Generic;
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


       
       private void RenderTransparent()
       {
              dragonEyeMesh.ForEach(x => x.material = dragonMatEyeTrans);
              dragonMesh.material = dragonMatTrans;
       }
       
       private void RenderOpaque()
       {
              dragonEyeMesh.ForEach(x => x.material = dragonMatEyeOpaque);
              dragonMesh.material = dragonMatOpaque;
       }

       private IEnumerator RenderTransAfterDelay(float delay)
       {
              yield return new WaitForSeconds(delay);
              RenderTransparent();
       }
       
       public void TakeHit()
       {
              dragonEyeMesh.ForEach(x => x.material = dragonMatEyeOpaque);
              dragonMesh.material = dragonMatHit;
              StartCoroutine(RenderTransAfterDelay(0.05f));
       }
}
