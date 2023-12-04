using System;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelectedEvent;
    public static event Action<TileData, CardInfoInstance> OnTilePosedEvent;
    public static event Action<CardInfoInstance> OnFinishToPose;
    public static event Action<CardHand> OnDiscardCardEvent; 


    [SerializeField] private GameObject gridVisualizer;
    private TileData selectedTile;
    private Vector3 offset;
    
    private Vector3 mousePos;
    
    [SerializeField] private GameObject discardPosition;
    
    public static MovementManager Instance { get; private set; }
    CardHand selectedCard;

    private void Start()
    {
        _rectTransform = discardPosition.GetComponent<RectTransform>();
    }

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
            card.GetImage().gameObject.transform.position = card.transform.position;
            card.removeSelection();
            return;
        }
        
        data.SetInstance(card.Card);
        OnTilePosedEvent?.Invoke(data, card.Card);
        //card.EmptyCard();
        OnFinishToPose?.Invoke(card.Card);
        selectedCard = null;
        card.GetImage().gameObject.transform.position = card.transform.position;
        // card.removeSelection();
        // selectedCard = null;
        // CardsManager.Instance.SetSelectedCard(null);
    }

    private void RotateSelection(bool direction)
    {
        if (selectedCard != null)
        {
            selectedCard.GetImage().transform.Rotate(0, 0, 90 * (direction ? 1 : -1));
            selectedCard.Card.AddRotation(direction);
        }
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
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            RotateSelection(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            RotateSelection(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            RotateSelection(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateSelection(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateSelection(true);
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

    private string nameOf = null;
    private RectTransform _rectTransform;

    private void HandleMouseDrag()
    {
        if (selectedCard == null) return;
        if (nameOf == null || nameOf != selectedCard.name)
        {
            nameOf = selectedCard.name;
            // selectedCard.ChangeSelection(false);
            // CardsManager.Instance.SetSelectedCard(null);
        }
        selectedCard.GetImage().gameObject.transform.position = Input.mousePosition;
    }


    private void HandleMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Input.mousePosition.x > _rectTransform.position.x - _rectTransform.sizeDelta.x / 2 &&
            Input.mousePosition.x < _rectTransform.position.x + _rectTransform.sizeDelta.x / 2 &&
            Input.mousePosition.y > _rectTransform.position.y - _rectTransform.sizeDelta.y / 2 &&
            Input.mousePosition.y < _rectTransform.position.y + _rectTransform.sizeDelta.y / 2)
        {
            if (selectedCard == null) return;
            selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
            OnDiscardCardEvent?.Invoke(selectedCard);
            return;
        }

        if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
        {
            if (selectedCard != null) selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
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