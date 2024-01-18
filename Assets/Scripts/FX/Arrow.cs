using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : AttackFX
{
    [SerializeField] Transform rect;
    private Transform tr;
    private float timer;

    public override void Init(Transform target, Transform owner, float time, DirectionToMove direction)
    {
        tr = target;
        transform.position = owner.position;
        Transform po = transform;
        po.LookAt(target);
        float DirectionToMove = 0;
        
        switch (direction)
        {
            case global::DirectionToMove.Up:
                DirectionToMove = 270;
                break;
            case global::DirectionToMove.Down:
                DirectionToMove = 90;
                break;
            case global::DirectionToMove.Left:
                DirectionToMove = 0;
                break;
            case global::DirectionToMove.Right:
                DirectionToMove = 180;
                break;
        }
        
        rect.rotation = Quaternion.Euler(90, 0, DirectionToMove);
        //il faut rotate en fonction de la direction
        timer = time;
    }

    public override void Launch()
    {
        transform.DOMove(tr.position, timer).SetEase(Ease.InBack);
        
        Destroy(gameObject, timer);
    }
}