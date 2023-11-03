using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapData : MonoBehaviour
{
    public abstract T GetSO<T>() where T : TrapSO;
    public abstract void TakeDamage(int damage);
    public bool isDead;
}
