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
            Vector3 dir = (anim.targetTransform.position - anim.obj.position).normalized;
            Vector3 targetPos = (anim.isDir) ? anim.targetTransform.position : anim.targetTransform.position+ anim.offset;
            anim.obj.DOMove(targetPos, anim.time).SetEase(anim.ease).SetLoops(anim.loop, LoopType.Yoyo);
            yield return new WaitForSeconds(anim.time + .1f);
        }
        isExec = false;
    }
}
