using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpawnRockOnDragon : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;
    private GameObject theRock;
    private Rigidbody theRockRb;
    private Material theRockMat;
    void Start()
    {
        //UI_Dragon.OnDragonTakeDamageEvent += SpawnRock;
        theRock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
        theRockRb = theRock.GetComponent<Rigidbody>();
        theRockMat = theRock.GetComponent<Material>();
    }
    
    private void OnDisable()
    {
        //UI_Dragon.OnDragonTakeDamageEvent -= SpawnRock;
    }
    
    private void SpawnRock()
    {
        //make a random rotation
        StartCoroutine(RockFall());
    }
    
    IEnumerator RockFall()
    {
        //yield return new WaitForSeconds(0.05f);
        Quaternion randRot = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        theRock.transform.position = transform.position;
        theRock.transform.localRotation = randRot;
        theRockRb.velocity = Vector3.zero;
        theRockRb.AddForce(Vector3.up * -1000f, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        theRock.transform.position = transform.position;
        theRockRb.velocity = Vector3.zero;
        
    }
}
