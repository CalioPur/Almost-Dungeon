using System;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelectedEvent;
    public static event Action<TileData, CardInfoInstance> OnTilePosedEvent;
    public static event Action OnFinishToPose;


    [SerializeField] private GameObject gridVisualizer;
    
    public TileData tileToPreview;
    
    private TileData selectedTile;
    private Vector3 offset;
    
    private Vector3 mousePos;
    
    public static DragAndDropManager Instance { get; private set; }
    CardHand selectedCard;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        MapManager.OnCardTryToPlaceEvent += PlaceCard;
    }

    private void OnDisable()
    {
        MapManager.OnCardTryToPlaceEvent -= PlaceCard;
        
    }

    private void PlaceCard(TileData data, CardHand card, bool canBePlaced)
    {
        if (!canBePlaced)
        {
            card.img.gameObject.transform.position = card.transform.position;
            card.removeSelection();
            return;
        }
        
        data.SetInstance(card.Card);
        OnTilePosedEvent?.Invoke(data, card.Card);
        //card.EmptyCard();
        OnFinishToPose?.Invoke();
        card.img.gameObject.transform.position = card.transform.position;
        card.removeSelection();
        selectedCard = null;
        CardsManager.Instance.SetSelectedCard(null);
    }

    void Update()
    {
        gridVisualizer.GetComponent<Renderer>().sharedMaterial.SetVector("_Position", GetMousePositionOnGrid());
        if (Input.GetMouseButtonDown(0))
        {
            // HandleMouseDown();
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
        OnTileSelectedEvent?.Invoke(tile);
    }

    private string name = null;
    private void HandleMouseDrag()
    {
        if (selectedCard == null) return;
        if (name == null || name != selectedCard.name)
        {
            name = selectedCard.name;
            // selectedCard.ChangeSelection(false);
            // CardsManager.Instance.SetSelectedCard(null);
        }
        selectedCard.img.gameObject.transform.position = Input.mousePosition;
    }


    private void HandleMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
        {
            selectedCard.img.gameObject.transform.position = selectedCard.transform.position;
            return;
        }
        TileData tile = hit.collider.gameObject.GetComponent<TileData>();
        OnTileSelectedEvent?.Invoke(tile);
    }

    public void SetSelectedCard(CardHand cardHand)
    {
        selectedCard = cardHand;
    }
}