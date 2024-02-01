using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectionFadeOut : MonoBehaviour
{
    [SerializeField] private Image img;
    void Start()
    {
        img.DOFade(0, 2f).SetUpdate(true);
    }
    
    
}
