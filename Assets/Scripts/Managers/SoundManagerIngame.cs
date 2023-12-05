using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    WallBreak,
    DragonDeath
}

public class SoundManagerIngame : MonoBehaviour
{
    public static SoundManagerIngame Instance;
    [SerializeField] private AudioSource audioSourceSurprise;
    [SerializeField] private AudioSource audioWallBreak;
    [SerializeField] private AudioSource deadDragon;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void PlaySound(AudioSource source)
    {
        if (source == null) return;
        if (source.isPlaying) return;
        source.Play();
    }
    
    public void PlaySound(EmoteType emote)
    {
        switch (emote)
        {
            case EmoteType.Detected:
                PlaySound(audioSourceSurprise);
                break;
            case EmoteType.Stuck:
                
                break;
        }
    }
    
    public void PlaySound(SoundType sound)
    {
        switch (sound)
        {
            case SoundType.WallBreak:
                PlaySound(audioWallBreak);
                break;
            case SoundType.DragonDeath:
                PlaySound(deadDragon);
                break;
        }
    }
}
