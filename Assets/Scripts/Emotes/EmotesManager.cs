using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class EmoteData
{
    public EmoteType type;
    public Emote emote;
}

[Serializable]
public enum EmoteType
{
    Detected,
}

public class EmotesManager : MonoBehaviour
{
    [SerializeField] private List<EmoteData> emotes = new();
    
    Dictionary<EmoteType, Emote> emoteDictionary = new Dictionary<EmoteType, Emote>();

    private void Start()
    {
        foreach (var emoteData in emotes)
        {
            emoteDictionary.Add(emoteData.type, emoteData.emote);
        }
    }

    public void PlayEmote(EmoteType type)
    {
        emoteDictionary[type].PlayEmote();
        SoundManagerIngame.Instance.PlaySound(type);
    }
    
    public void StopEmote(EmoteType type)
    {
        emoteDictionary[type].StopEmote();
    }
}
