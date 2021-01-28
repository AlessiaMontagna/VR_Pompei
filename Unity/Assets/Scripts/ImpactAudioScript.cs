using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudioScript : MonoBehaviour
{
    public AudioClip ImpactAudio;
    AudioSource audio;
    public bool AlreadyPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!AlreadyPlayed)
        {
            audio.PlayOneShot(ImpactAudio);
            AlreadyPlayed = true;
        }
    }
}
