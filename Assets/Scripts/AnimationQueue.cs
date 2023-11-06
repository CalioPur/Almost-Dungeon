using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationQueue : MonoBehaviour
{
    private Queue<AnimToQueue> animQueue = new ();
    private bool isExec = false;
    public void AddAnim(AnimToQueue anim)
    {
        animQueue.Enqueue(anim);
        if (!isExec)
        {
            StartCoroutine(doAnim());
        }
    }
    private IEnumerator doAnim()
    {
        isExec = true;
        while (animQueue.Count > 0)
        {
            AnimToQueue anim = animQueue.Dequeue();
            anim.obj.DOMove(anim.target, anim.time).SetEase(anim.ease).SetLoops(anim.loop, LoopType.Yoyo);
            yield return new WaitForSeconds(anim.time);
        }
        isExec = false;
    }
}
