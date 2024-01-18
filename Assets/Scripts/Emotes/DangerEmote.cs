using System.Collections;
using UnityEngine;

public class DangerEmote : Emote
{
    [SerializeField] private SpriteRenderer surpriseEmote;
    
    public override void PlayEmote()
    {
        if (!gameObject.activeSelf) return;
        surpriseEmote.enabled = true;
        StartCoroutine(DisableEmote());
    }

    IEnumerator DisableEmote()
    {
        yield return new WaitForSeconds(2.5f);
        surpriseEmote.enabled = false;
    }
    
    public override void StopEmote()
    {
        surpriseEmote.enabled = false;
    }
}
