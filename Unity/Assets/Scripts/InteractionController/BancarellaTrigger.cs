using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BancarellaTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public int index = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Schiavo")
        {
            audioSource.clip = Resources.Load<AudioClip>("Talking/Trigger_Mercante_Frutta" + index);
            audioSource.Play();
        }
    }
}

