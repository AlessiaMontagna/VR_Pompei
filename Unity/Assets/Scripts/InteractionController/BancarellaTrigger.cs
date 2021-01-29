using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BancarellaTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Schiavo")
        {
            audioSource.clip = Resources.Load<AudioClip>("Talking/SchiavoStaticDialogue");
            audioSource.Play();
        }
    }
}

