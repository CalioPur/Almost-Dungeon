using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerIngame : MonoBehaviour
{
    public static SoundManagerIngame Instance;
    [SerializeField] private AudioSource audioSourceSurprise;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void PlaySound(EmoteType emote)
    {
        AudioSource source = null;
        switch (emote)
        {
            case EmoteType.Detected:
                
                source = audioSourceSurprise;
                break;
        }
        if (source == null) return;
        if (source.isPlaying) return;
        source.Play();
    }
}
