 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackFX : MonoBehaviour
{
 public abstract void Launch();
 public abstract void Init(Transform target, Transform owner, float time);
}
