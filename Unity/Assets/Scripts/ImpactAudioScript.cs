using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudioScript : MonoBehaviour
{
    public AudioClip ImpactAudio;
    AudioSource _audioSource;
    public bool AlreadyPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!AlreadyPlayed)
        {
            _audioSource.PlayOneShot(ImpactAudio);
            AlreadyPlayed = true;
        }
    }
}
