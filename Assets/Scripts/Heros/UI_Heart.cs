using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Heart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart;
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