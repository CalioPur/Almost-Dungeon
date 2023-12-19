using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Slash : AttackFX
{
    [SerializeField] Animator anim;

    public override void Launch()
    {
        anim.SetTrigger("Attack");
        Destroy(gameObject, 0.2f);
    }

    public override void Init(Transform target, Transform owner, float time)
    {
        
    }
}