using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourbilol : AttackFX
{
    [SerializeField] private Animator anim;

    public override void Init(Transform target, Transform owner, float time, DirectionToMove direction)
    {
        transform.position = owner.position + Vector3.up * 0.1f;
        Destroy(gameObject, 0.5f);
    }

    public override void Launch()
    {
        anim.SetTrigger("Launch");
        
       
    }
}
