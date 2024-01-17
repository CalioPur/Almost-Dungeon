using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpawnRockOnDragon : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;
    private GameObject theRock;
    void Start()
    {
        UI_Dragon.OnDragonTakeDamageEvent += SpawnRock;
        theRock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
    }
    
    private void OnDisable()
    {
        UI_Dragon.OnDragonTakeDamageEvent -= SpawnRock;
    }
    
    private void SpawnRock()
    {
        //make a random rotation
        StartCoroutine(RockFall());
    }
    
    IEnumerator RockFall()
    {
        yield return new WaitForSeconds(0.15f);
        Quaternion randRot = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        theRock.transform.localRotation = randRot;
        theRock.transform.position = transform.position;
        theRock.GetComponent<Rigidbody>().AddForce(Vector3.up * -1000f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        theRock.GetComponent<Material>().DOColor(Color.clear, 0.5f);
    }
}
