using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CardToMapManager : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Canvas canvas;
    private List<GameObject> textsValueForExploration = new();

    private GameObject text;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tpos = tileMap.WorldToCell(worldPoint);


            TileBase tileFound = tileMap.GetTile(tpos);
            if (!tileFound) return;
            if (!GameManager.Instance.IsTileCanBeOccupied(tpos, DeckManager.Instance.GetDoorsSelectedCard()))
            {
                Debug.Log("Tile is occupied");
                return;
            }
            
            try
            {
                
                TileType type = DeckManager.Instance.getTypeSelectedCard();
                Debug.Log(type);
                GameManager.Instance.SpawnTile(tpos, type);
                tileFound = tileMap.GetTile(tpos);

                Matrix4x4 tileTransform =
                    Matrix4x4.Translate(new Vector3(0, 0)) *
                    Matrix4x4.Rotate(Quaternion.Euler(0, 0, DeckManager.Instance.GetRotationSelectedCard().y));
                
                TileChangeData tilechangedata = new TileChangeData()
                {
                    position = tpos,
                    tile = tileFound,
                    color = Color.white,
                    transform = tileTransform
                };
                tileMap.SetTile(tilechangedata, false);
                
                GameManager.Instance.OccupiedTile(tpos, DeckManager.Instance.GetDoorsSelectedCard(), DeckManager.Instance.getTypeSelectedCard());
                DeckManager.Instance.DiscardCurrentCard();
            }
            catch 
            {
                Debug.LogWarning("No card Selected");
            }

        }
        
    }
}
