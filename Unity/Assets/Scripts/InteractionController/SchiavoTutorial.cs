using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchiavoTutorial : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void Talk(AudioClip audio)
    {
        audioSource.clip = audio;
        if(!audioSource.isPlaying) audioSource.Play();

    }
}
