using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class AnimToQueue
{
    public Transform obj;
    public Vector3 dir;
    public float time;
    public Ease ease;
    public int loop;
    public bool isDir;

    public AnimToQueue(Transform obj, Vector3 dir, bool isDir, float time, Ease ease = Ease.Linear, int loop = 1)
    {
        this.obj = obj;
        this.dir = dir;
        this.time = time;
        this.ease = ease;
        this.loop = loop;
        this.isDir = isDir;
    }

}
