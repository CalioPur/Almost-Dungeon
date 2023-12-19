using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamehameHa : AttackFX
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Animator anim;
    public void BackTo()
    {
        rect.localPosition = Vector3.right;
        parent.localPosition += Vector3.left * 6;
        anim.SetTrigger("Back");
       
    }

    public override void Launch()
    {
        anim.SetTrigger("Attack");
    }

    public override void Init(Transform target, Transform owner, float time, DirectionToMove direction)
    {
        rect.localPosition = Vector3.left;
        parent.localScale = Vector3.right * 0.1f;
        parent.localPosition = owner.position;
        Destroy(gameObject, 0.5f);
    }
}
