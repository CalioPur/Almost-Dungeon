using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Slash : AttackFX
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject RenderAnim;
    
    private float timer;

    public override void Launch()
    {
        anim.SetTrigger("Attack");
        Destroy(gameObject, 0.7f);
    }

    public override void Init(Transform target, Transform owner, float time, DirectionToMove direction)
    {
        timer = time;
        transform.position = target.position;
        //RenderAnim.SetActive(false);
    }
}