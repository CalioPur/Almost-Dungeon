using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EmotesManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer emotePlayer;
    [SerializeField] private GameObject textEmote;
    [SerializeField] private List<VideoClip> emotes;
    
    
    public IEnumerator PlayEmote(int index)
    {
        textEmote.SetActive(true);
        if (index < 0 || index >= emotes.Count) yield break;
        emotePlayer.clip = emotes[index];
        emotePlayer.Play();
        yield return new WaitForSeconds((float)emotes[index].length);
        textEmote.SetActive(false);
    }
    
    public void StopEmote(int index)
    {
        if (index < 0 || index >= emotes.Count) return;
        emotePlayer.Stop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PlayEmote(0));
        }
    }
}
