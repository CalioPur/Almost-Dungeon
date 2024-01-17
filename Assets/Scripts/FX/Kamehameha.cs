using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamehameha : MonoBehaviour
{
    public Vector3 targetPos;
    
    [SerializeField] private LineRenderer lineRenderer;

    void OnEnable()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPos);
    }
}