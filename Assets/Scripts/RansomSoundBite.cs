using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;
using Time = UnityEngine.Time;

[RequireComponent(typeof(AudioSource))]
public class RansomSoundBite : MonoBehaviour
{
    public List<AudioClip> Sounds;
    protected AudioSource AudioSource;
    
    // Start is called before the first frame update
    protected void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public AudioClip Play()
    {
        var audioClip = Sounds[Random.Range(0, Sounds.Count)];
        AudioSource.clip = audioClip;
        AudioSource.Play();
        return audioClip;
    }
}
