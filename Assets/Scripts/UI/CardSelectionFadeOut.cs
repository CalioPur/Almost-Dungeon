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
        StartCoroutine(FadeOut());
        
    }
    
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        img.DOFade(0, 2f).SetUpdate(true);
    }
    
    
}
