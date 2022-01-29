using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;
using Time = UnityEngine.Time;

public class ProximitySoundBite : MonoBehaviour
{
    public List<AudioClip> Sounds;
    public float Cooldown = 5;

    // Next time at which we allow playing a sound bite.
    private float ReadyTime = 0;
    private AudioSource AudioSource;
    
    // Time it takes to scale up when starting/stopping sound
    public float ScaleTime = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Scale the model when playing sound
        float lo = 1f;
        float hi = 1.3f;
        float targetScale = AudioSource.isPlaying ? hi : lo;
        float currentScale = transform.localScale.x;
        float candidateScaleDelta = targetScale - currentScale;
        float maxAbsScaleRate = (hi - lo) / ScaleTime;
        float maxAbsScaleDelta = maxAbsScaleRate * Time.deltaTime;
        if (Math.Abs(candidateScaleDelta) > maxAbsScaleDelta)
        {
            float scaleDelta = candidateScaleDelta > 0 ? maxAbsScaleDelta : -maxAbsScaleDelta;
            transform.localScale = Vector3.one * (currentScale + scaleDelta);
        }
        else
        {
            transform.localScale = Vector3.one * targetScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player) && Time.time >= ReadyTime)
        {
            // Play the sound
            var audioClip = Sounds[Random.Range(0, Sounds.Count)];
            ReadyTime = Time.time + Cooldown + audioClip.length;
            AudioSource.clip = audioClip;
            AudioSource.Play();
        }
    }
}
