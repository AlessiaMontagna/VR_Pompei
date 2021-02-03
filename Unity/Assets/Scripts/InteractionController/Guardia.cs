﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardia : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    System.Random rnd;

    public void Talk(GameObject caller)
    {
        rnd = new System.Random();
        int index = rnd.Next(1, 4);
        if (!audioSource.isPlaying)
        {
            switch (caller.tag)
            {
                case "Player_Nobile":
                    //Audio nobili: prendi negli asset la stringa corrispondente a "Nobile_NobileM + (index)"
                    audioSource.clip = Resources.Load<AudioClip>("Talking/Nobile_Guardia" + index.ToString());
                    audioSource.Play();
                    break;
                case "Player_Schiavo":
                    // AUdio schiavo: prendi negli asset la stringa corrispondente a "Schiavo_NobileM + (index)"
                    audioSource.clip = Resources.Load<AudioClip>("Talking/Schiavo_Guardia" + index.ToString());
                    audioSource.Play();
                    break;
            }
        }
    }
}
