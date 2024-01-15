using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelectedEvent;
    public static event Action<TileData, CardInfoInstance> OnTilePosedEvent;
    public static event Action<CardInfoInstance> OnFinishToPose;

    //[SerializeField] private GameObject gridVisualizer;
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
        if (!canBePlaced) return;
        
        cardVisualizer.SetActive(false);
        data.SetInstance(card.Card);
        OnTilePosedEvent?.Invoke(data, card.Card);
        //card.EmptyCard();
        OnFinishToPose?.Invoke(card.Card);
        cardVisualizer.transform.rotation = Quaternion.Euler(baseRotation);
        cardVisualizer.SetActive(false);
        selectedCard = null;
        Debug.Log("Set selected card null");
    }

    IEnumerator RotateB(float time, float desiredAngle)
    {
        float ratio = -90.0f / time;
        
        while (time > 0)
        {
            time -= Time.deltaTime;
            Vector3 rot = selectedCard.GetImage().transform.rotation.eulerAngles;
            rot += new Vector3(0, 0, ratio * Time.deltaTime);
            Vector3 rot2 = cardVisualizer.transform.rotation.eulerAngles;
            rot2 += new Vector3(0, 0, ratio * Time.deltaTime);
            selectedCard.GetImage().transform.rotation = Quaternion.Euler(rot);
            cardVisualizer.transform.rotation = Quaternion.Euler(rot2);
            yield return null;
        }
        selectedCard.GetImage().transform.rotation = Quaternion.Euler(new Vector3(0, 0, desiredAngle));
        cardVisualizer.transform.rotation = Quaternion.Euler(new Vector3(90, 0, desiredAngle));
    }

    private void RotateSelection(bool direction)
    {
        if (selectedCard != null)
        {
            selectedCard.Card.AddRotation(direction);
            StartCoroutine(RotateB(0.2f, selectedCard.Card.Rotation));
        }
    }
    
    private Color redA05 = new(255, 0, 0, 0.5f);
    private Color invisible = new(255, 255, 255, 0);
    private Color whiteA05 = new(255, 255, 255, 0.5f);
    void Update()
    {
        //gridVisualizer.GetComponent<Renderer>().sharedMaterial.SetVector("_Position", GetMousePositionOnGrid());

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
    

    private void HandleMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
        {
            if (selectedCard != null) selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
            return;
        }
        
        if (selectedCard != null)
        {
            TileData tile = hit.collider.gameObject.GetComponent<TileData>();
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
        if (!selectedCard) return;
        if (selectedCard.GetSprite() == null)
        {
            Debug.Log("Sprite null");
            return;
        }
        cardVisualizer.GetComponent<SpriteRenderer>().sprite = selectedCard.GetSprite();
    }
}