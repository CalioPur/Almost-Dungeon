using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseEmote : Emote
{
    [SerializeField] private SpriteRenderer surpriseEmote;
    
    public override void PlayEmote()
    {
        surpriseEmote.enabled = true;
        StartCoroutine(DisableEmote());
    }

    IEnumerator DisableEmote()
    {
        yield return new WaitForSeconds(1f);
        surpriseEmote.enabled = false;
    }
    
    public override void StopEmote()
    {
        surpriseEmote.enabled = false;
    }
}
