using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : AttackFX
{
    [SerializeField] RectTransform rect;
    private Transform tr;
    private float timer;

    public override void Init(Transform target, Transform owner, float time)
    {
        tr = target;
        transform.position = owner.position;
        rect.rotation = Quaternion.Euler(90, 0,
            Vector3.SignedAngle(Vector3.right, target.position - owner.position, Vector3.forward));
        timer = time;
    }

    public override void Launch()
    {
        transform.DOMove(tr.position, timer).SetEase(Ease.InBack);
        Destroy(gameObject, timer);
    }
}