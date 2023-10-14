using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickerFou : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         
            var tpos = tileMap.WorldToCell(worldPoint);

            // Try to get a tile from cell position
            var tile = tileMap.GetTile(tpos);

            if(tile)
            {
                Debug.Log("Tile found at " + tpos);
            }
        }
    }
}
