using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DestroyTileEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float value = 30000.0f;
        float delay = 0.5f;
        Destroy(gameObject, delay);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        
        rb.AddForce(new Vector3(Random.Range(-value, value), 
            value * 100.0f, 
            Random.Range(-value, value)), ForceMode.Impulse);
        // transform.DOMove(transform.position + 
        //                  new Vector3(Random.Range(-value, value), 
        //                      Random.Range(1.0f, value), 
        //                      Random.Range(-value, value)), delay * 0.9f).SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(0, 0.1f, 0);
    }
}
