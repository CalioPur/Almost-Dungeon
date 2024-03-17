using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatDragon : MonoBehaviour
{
       [SerializeField] private Material dragonMatTrans;
       [SerializeField] private Material dragonMatOpaque;
       [SerializeField] private MeshRenderer dragonMesh;
       
       [SerializeField] private Material dragonMatEyeTrans;
       [SerializeField] private Material dragonMatEyeOpaque;
       [SerializeField] private List<MeshRenderer> dragonEyeMesh;


       
       public void RenderTransparent()
       {
              dragonEyeMesh.ForEach(x => x.material = dragonMatEyeTrans);
              dragonMesh.material = dragonMatTrans;
       }
       
       public void RenderOpaque()
       {
              dragonEyeMesh.ForEach(x => x.material = dragonMatEyeOpaque);
              dragonMesh.material = dragonMatOpaque;
       }
}
