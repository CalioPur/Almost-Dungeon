using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AudioDataSound
{
    public EmoteType emote;
    public AudioSource audioSource;
}

[Serializable]
public struct SFXAudioDataSound
{
    public String sfxName;
    public AudioSource audioSource;
}

public class SoundManagerIngame : MonoBehaviour
{
    public static SoundManagerIngame Instance;
    [SerializeField] private List<AudioDataSound> emotesAudioSources;
    public SFXAudioDataSound[] sfxAudioSource;
    
    private Dictionary<EmoteType, AudioSource> emotesDictionary = new Dictionary<EmoteType, AudioSource>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    
    private void Start()
    {
        foreach (var audioDataSound in emotesAudioSources)
        {
            emotesDictionary.Add(audioDataSound.emote, audioDataSound.audioSource);
        }
    }

    private void PlaySound(AudioSource source)
    {
        if (source == null) return;
        if (source.isPlaying) return;
        source.Play();
    }
    
    public void PlaySound(EmoteType emote)
    {
        PlaySound(emotesDictionary[emote]);
    }

    public void PlayDialogueSFX(string name)
    {
        SFXAudioDataSound s = Array.Find(sfxAudioSource, x => x.sfxName == name);
        
            PlaySound(s.audioSource);
    }
}
