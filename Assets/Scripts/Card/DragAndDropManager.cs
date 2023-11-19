using System;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelectedEvent;
    public static event Action<TileData, CardInfoInstance> OnTilePosedEvent;

    [SerializeField] private GameObject gridVisualizer;
    
    public TileData tileToPreview;
    
    private TileData selectedTile;
    private Vector3 offset;
    
    private Vector3 mousePos;

    private void Start()
    {
        MapManager.OnCardTryToPlaceEvent += PlaceCard;
    }

    private void PlaceCard(TileData data, CardHand card, bool canBePlaced)
    {
        if (!canBePlaced) return;

        data.SetInstance(card.Card);
        OnTilePosedEvent?.Invoke(data, card.Card);
        card.EmptyCard();
    }

    void Update()
    {
        gridVisualizer.GetComponent<Renderer>().material.SetVector("_Position", GetMousePositionOnGrid());
        tileToPreview = selectedTile;
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            HandleMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }
    
    private Vector4 GetMousePositionOnGrid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return Vector4.zero;
        if (!hit.collider.gameObject == gridVisualizer) return Vector4.zero;
        mousePos = hit.point;
        //set the mouse position to a value between 0 and 1
        mousePos.x /= (gridVisualizer.transform.localScale.x*10);
        mousePos.z /= (gridVisualizer.transform.localScale.z*10);
        mousePos.x = 0.5f - mousePos.x;
        mousePos.z = 0.5f - mousePos.z;
        mousePos.x = Mathf.Clamp(mousePos.x, 0, 1);
        mousePos.z = Mathf.Clamp(mousePos.z, 0, 1);
        return new Vector4(mousePos.x, mousePos.z, 0, 0);
    }

    private void HandleMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor")) return;
        TileData tile = hit.collider.gameObject.GetComponent<TileData>();
        // tileToPreview = tile;
        if (tile.PiecePlaced)
        {
            // Store the selected tile and calculate the offset
            selectedTile = tile;
            offset = hit.point - tile.transform.position;
        }
        else
        {
            // Invoke the tile selected event if no piece is placed on the tile
            OnTileSelectedEvent?.Invoke(tile);
        }
    }

    private void HandleMouseDrag()
    {
        if (selectedTile == null) return;
        // Update the position of the selected tile during dragging
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Floor"))
        {
            selectedTile.transform.position = hit.point - offset;
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        Debug.DrawLine(ray.origin, hit.point, Color.blue);
    }

    private void HandleMouseUp()
    {
        if (selectedTile == null) return;
        // Snap the selected tile to the grid position
        selectedTile.SnapToGrid();

        // Invoke the tile posed event with the updated tile data
        OnTilePosedEvent?.Invoke(selectedTile, selectedTile.CardInstance);

        // Reset the selected tile
        selectedTile = null;
    }
}