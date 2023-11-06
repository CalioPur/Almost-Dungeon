using UnityEngine;
using UnityEngine.UI;


public class UI_Heart : MonoBehaviour
{
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;
    
    private Image heartImage;
    
    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }
    
    public void SetHeartState(HeartState state)
    {
        heartImage.sprite = state switch
        {
            HeartState.Full => fullHeart,
            HeartState.Half => halfHeart,
            HeartState.Empty => emptyHeart,
            _ => heartImage.sprite
        };
    }
}
public enum HeartState
{
    Full = 2,
    Half = 1,
    Empty = 0
}