using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class AnimToQueue : MonoBehaviour
{
    public Transform obj;
    public Vector3 target;
    public float time;
    public Ease ease;
    public int loop;

    public AnimToQueue(Transform obj, Vector3 target, float time, Ease ease = Ease.Linear, int loop = 1)
    {
        this.obj = obj;
        this.target = target;
        this.time = time;
        this.ease = ease;
        this.loop = loop;
    }

}
