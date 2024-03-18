using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTileEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0.1f, 0);
    }
}
