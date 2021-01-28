using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amico : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player_Nobile")
        {
            audioSource.clip = Resources.Load<AudioClip>("Talking/Nobile_Mercante1");
            audioSource.Play();
        }
    }
}
