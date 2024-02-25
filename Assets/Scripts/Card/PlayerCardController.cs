using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerCardController : MonoBehaviour
{
    public static event Action<TileData> OnTileSelectedEvent;
    public static event Action<TileData, CardInfoInstance> OnTilePosedEvent;
    public static event Action<CardInfoInstance> OnFinishToPose;

    //[SerializeField] private GameObject gridVisualizer;
    [SerializeField] private SpriteRenderer cardVisualizer;
    
    private TileData selectedTile;
    private Vector3 offset;
    
    private Vector3 mousePos;
    
    Vector3 baseRotation = new(90, 0, 0);

    [SerializeField] private SoundManagerIngame soundManagerIngame;
    
    public static PlayerCardController Instance { get; private set; }
    CardHand selectedCard;
    
    public bool isDragNDrop = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        isDragNDrop = PlayerPrefs.GetInt("DragNDrop", 0) == 1;
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
        
        if (canBePlaced && TutorialManager.Instance != null)
        {
            canBePlaced = TutorialManager.Instance.CanBePlaced(data);
        }
        if (!canBePlaced) return;
        
        cardVisualizer.gameObject.SetActive(false);
        data.SetInstance(card.Card);
        OnTilePosedEvent?.Invoke(data, card.Card);
        OnFinishToPose?.Invoke(card.Card);
        cardVisualizer.transform.rotation = Quaternion.Euler(baseRotation);
        cardVisualizer.gameObject.SetActive(false);
        selectedCard = null;
        soundManagerIngame.PlayDialogueSFX("UiNegativeClick");
    }

    IEnumerator RotateB(float time, float desiredAngle, float direction)
    {
        float ratio = direction * 90.0f / time;
        
        while (time > 0)
        {
            if (selectedCard == null) yield break;
            time -= Time.deltaTime;
            Vector3 rot = selectedCard.GetImage().transform.rotation.eulerAngles;
            rot += new Vector3(0, 0, ratio * Time.deltaTime);
            Vector3 rot2 = cardVisualizer.transform.rotation.eulerAngles;
            rot2 += new Vector3(0, 0, ratio * Time.deltaTime);
            selectedCard.GetImage().transform.rotation = Quaternion.Euler(rot);
            cardVisualizer.transform.rotation = Quaternion.Euler(rot2);
            yield return null;
        }

        if (selectedCard == null) yield break;
        selectedCard.GetImage().transform.rotation = Quaternion.Euler(new Vector3(0, 0, desiredAngle));
        cardVisualizer.transform.rotation = Quaternion.Euler(new Vector3(90, 0, desiredAngle));
    }

    private void RotateSelection(bool direction)
    {
        if (selectedCard != null)
        {
            selectedCard.Card.AddRotation(direction);
            StartCoroutine(RotateB(0.2f, selectedCard.Card.Rotation, direction ? 1 : -1));
        }
    }
    
    private Color redA05 = new(255, 0, 0, 0.5f);
    private Color invisible = new(255, 255, 255, 0);
    private Color whiteA05 = new(255, 255, 255, 0.5f);
    void Update()
    {
        if (selectedCard != null) //si on a une carte dans la main
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
            {
                cardVisualizer.color = invisible;
            }
            TileData tile = hit.collider?.gameObject.GetComponent<TileData>();
            if (tile != null)
            {
                var position = tile.transform.position;
                cardVisualizer.transform.position = new Vector3(position.x, position.y + 0.3f, position.z);
                cardVisualizer.color = tile.PiecePlaced ? redA05 : whiteA05;
            }
        }
        
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

        if (!Physics.Raycast(ray, out hit) || !hit.collider.gameObject.CompareTag("Floor"))
        {
            if (selectedCard != null) selectedCard.GetImage().gameObject.transform.position = selectedCard.transform.position;
            return;
        }
        TileData tile = hit.collider.gameObject.GetComponent<TileData>();
        cardVisualizer.transform.rotation = Quaternion.Euler(baseRotation);
        cardVisualizer.gameObject.SetActive(false);

        OnTileSelectedEvent?.Invoke(tile);
    }

    public void SetSelectedCard(CardHand cardHand)
    {
        selectedCard = cardHand;
        soundManagerIngame.PlayDialogueSFX("UiNegativeClick");
        if (!selectedCard) return;
        cardVisualizer.transform.rotation =
            Quaternion.Euler(baseRotation - selectedCard.Card.Rotation * Vector3.up); //recupere la rotation de la carte quand on clique pour afficher correctement la preview
        cardVisualizer.transform.position = new Vector3(100, 100, 100);
        cardVisualizer.gameObject.SetActive(true);
        
        if (selectedCard.GetSprite() == null)
        {
            return;
        }
        cardVisualizer.sprite = selectedCard.GetSprite();
    }
}