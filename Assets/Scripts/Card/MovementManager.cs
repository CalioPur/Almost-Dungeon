using System;
using DG.Tweening;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelectedEvent;
    public static event Action<TileData, CardInfoInstance> OnTilePosedEvent;
    public static event Action<CardInfoInstance> OnFinishToPose;
    public static event Action<CardHand> OnDiscardCardEvent; 


    [SerializeField] private GameObject gridVisualizer;
    [SerializeField] private GameObject cardVisualizer;
    private TileData selectedTile;
    private Vector3 offset;
    
    private Vector3 mousePos;
    
    [SerializeField] private GameObject discardPosition;
    
    Vector3 baseRotation = new(90, 0, 0);
    
    public static MovementManager Instance { get; private set; }
    CardHand selectedCard;
    
    public bool isDragNDrop = false;

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
        cardVisualizer.transform.rotation = Quaternion.Euler(baseRotation);
        cardVisualizer.SetActive(false);
        selectedCard = null;
        Debug.Log("Set selected card null");
    }

    private void RotateSelection(bool direction)
    {
        if (selectedCard != null)
        {
            Vector3 rot = selectedCard.GetImage().transform.rotation.eulerAngles;
            selectedCard.Card.AddRotation(direction);
            selectedCard.GetImage().transform.DORotate(new Vector3(0, 0, selectedCard.Card.Rotation), 0.2f);
            cardVisualizer.transform.DORotate(new Vector3(90, 0, selectedCard.Card.Rotation), 0.2f);
        }
    }
    
    private Color redA05 = new(255, 0, 0, 0.5f);
    private Color invisible = new(255, 255, 255, 0);
    private Color whiteA05 = new(255, 255, 255, 0.5f);
    void Update()
    {
        gridVisualizer.GetComponent<Renderer>().sharedMaterial.SetVector("_Position", GetMousePositionOnGrid());

        if (selectedCard != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
            {
                cardVisualizer.GetComponent<SpriteRenderer>().color = invisible;
            }
            // TileData tile = hit.collider.gameObject.GetComponent<TileData>();
            TileData tile = hit.collider?.gameObject.GetComponent<TileData>();
            if (tile != null)
            {
                var position = tile.transform.position;
                cardVisualizer.transform.position = new Vector3(position.x, position.y + 0.3f, position.z);
                cardVisualizer.GetComponent<SpriteRenderer>().color = tile.PiecePlaced ? redA05 : whiteA05;
            }
        }
        // else
        // {
        //     Debug.Log("selectedCard null");
        // }
        
        switch (isDragNDrop)
        {
            case false when Input.GetMouseButtonDown(0):
                HandleMouseDown();
                break;
            case true when Input.GetMouseButton(0):
                HandleMouseDrag();
                break;
            case true when Input.GetMouseButtonUp(0):
                HandleMouseUp();
                break;
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
        
        //if the mouse pos intersects with the zone of the discard
        // if (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, Input.mousePosition))
        // {
        //     if (selectedCard == null) return;
        //     selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
        //     OnDiscardCardEvent?.Invoke(selectedCard);
        //     return;
        // }

        if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
        {
            if (selectedCard != null) selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
            // selectedCard = null;
            // Debug.Log("Set selected card null");
            return;
        }
        
        if (selectedCard != null)
        {
            TileData tile = hit.collider.gameObject.GetComponent<TileData>();
            cardVisualizer.transform.rotation = Quaternion.Euler(baseRotation);
            cardVisualizer.SetActive(false);
            OnTileSelectedEvent?.Invoke(tile);
        }
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
        
        //if the mouse pos intersects with the zone of the discard
        // if (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, Input.mousePosition))
        // {
        //     if (selectedCard == null) return;
        //     selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
        //     OnDiscardCardEvent?.Invoke(selectedCard);
        //     return;
        // }

        if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
        {
            if (selectedCard != null) selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
            // selectedCard = null;
            // Debug.Log("Set selected card null");
            return;
        }
        TileData tile = hit.collider.gameObject.GetComponent<TileData>();
        cardVisualizer.transform.rotation = Quaternion.Euler(baseRotation);
        cardVisualizer.SetActive(false);
        OnTileSelectedEvent?.Invoke(tile);
    }

    public void SetSelectedCard(CardHand cardHand)
    {
        selectedCard = cardHand;
        cardVisualizer.transform.rotation = Quaternion.Euler(baseRotation);
        cardVisualizer.transform.position = new Vector3(100, 100, 100);
        cardVisualizer.SetActive(true);
        if (selectedCard != null && selectedCard.GetSprite() == null)
        {
            Debug.Log("Sprite null");
            return;
        }
        cardVisualizer.GetComponent<SpriteRenderer>().sprite = selectedCard.GetSprite();
    }
}