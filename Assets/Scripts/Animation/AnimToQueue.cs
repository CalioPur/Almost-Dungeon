using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class AnimToQueue
{
    public Transform obj;
    public Transform targetTransform;
    public Vector3 offset;
    public float time;
    public Ease ease;
    public int loop;
    public bool isDir;

    public AnimToQueue(Transform obj,Transform _targetTransform, Vector3 _offset, bool isDir, float time, Ease ease = Ease.Linear, int loop = 1 ) {
        this.obj = obj;
        targetTransform = _targetTransform;
        offset = _offset;
        this.time = time;
        this.ease = ease;
        this.loop = loop;
        this.isDir = isDir;
    }

}
