using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraScreenShake : MonoBehaviour
{
    [SerializeField] float amplitude = 0.1f;
    [SerializeField] float duration = 0.2f;
    
    void Start()
    {
        UI_Dragon.OnDragonTakeDamageEvent += DoScreenShake;
    }

    private void OnDisable()
    {
        UI_Dragon.OnDragonTakeDamageEvent -= DoScreenShake;
    }
    
    private void DoScreenShake()
    {
        StartCoroutine(Shake());
    }
    
    IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * amplitude;
            float y = Random.Range(-1f, 1f) * amplitude;
            float z = Random.Range(-1f, 1f) * amplitude;

            transform.localPosition = new Vector3(originalPos.x+x, originalPos.y+y, originalPos.z+z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
