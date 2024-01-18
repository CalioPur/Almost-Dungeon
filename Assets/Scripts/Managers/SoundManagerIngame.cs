using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public AudioSource sfxAudioSource;
}

public class SoundManagerIngame : MonoBehaviour
{
    public static SoundManagerIngame Instance;
    [SerializeField] private List<AudioDataSound> emotesAudioSources;
    [SerializeField] private List<SFXAudioDataSound> sfxAudioSourceList;
    //public SFXAudioDataSound[] sfxAudioSourceList;
    
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
        //if (source.isPlaying) return;
        source.Play();
    }
    
    public void PlaySound(EmoteType emote)
    {
        PlaySound(emotesDictionary[emote]);
    }

    public void PlayDialogueSFX(string key)
    {
        Debug.Log($"{key}");
        if (sfxAudioSourceList == null) return;
        
        if (sfxAudioSourceList.Count <= 0) return;
        
        AudioSource s = sfxAudioSourceList.FirstOrDefault(x  => x.sfxName == key).sfxAudioSource;
        
        if (s == null) return;
        
        PlaySound(s);
        
        // if (sfxAudioSourceList.ContainsKey((EmoteType)Enum.Parse(typeof(EmoteType), key)))
        // {
        //     PlaySound(emotesDictionary[(EmoteType)Enum.Parse(typeof(EmoteType), key)]);
        //     return;
        // }
        // SFXAudioDataSound s = sfxAudioSourceList.FirstOrDefault(x  => x.sfxName == key);
        //
        // Debug.Log($"{s}");
        //
        // PlaySound(s.sfxAudioSource);
    }

    [ContextMenu("TEST")]
    private void test()
    {
        PlayDialogueSFX("DragonRoar");
    }
}
