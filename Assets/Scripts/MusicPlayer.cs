using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> audioClips;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        audioSource.clip = audioClips[DungeonManager.SelectedBiome];
        audioSource.Play();
    }
}
