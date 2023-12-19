using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimationQueue : MonoBehaviour
{
    private Queue<AnimToQueue> animQueue = new();
    private bool isExec = false;
    private Coroutine coroutine;

    public void AddAnim(AnimToQueue anim)
    {
        if (!gameObject.activeSelf) return;
            animQueue.Enqueue(anim);
        if (!isExec)
        {
            coroutine = StartCoroutine(doAnim());
        }
    }

    private IEnumerator doAnim()
    {
        isExec = true;
        while (animQueue.Count > 0)
        {
            AnimToQueue anim = animQueue.Dequeue();
            Vector3 dir = (anim.targetTransform.position - anim.obj.position).normalized;
            Vector3 targetPos =
                (anim.isDir) ?  anim.obj.position + dir * 0.5f : anim.targetTransform.position + anim.offset;
            anim.obj.DOMove(targetPos, anim.time).SetEase(anim.ease).SetLoops(anim.loop, LoopType.Yoyo);
            if (anim.mustDisappear)
            {
                anim.obj.GetComponent<Material>().DOColor(Color.clear, anim.time).SetEase(anim.ease)
                    .SetLoops(anim.loop, LoopType.Yoyo);
            }
            yield return new WaitForSeconds(anim.time + .1f);
            if (!gameObject.activeSelf) yield break;
        }

        isExec = false;
    }

    private void OnEnable()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = null;
        animQueue.Clear();
    }

    private void OnDisable()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = null;
        animQueue.Clear();
    }
}