using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amico : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public System.Action Mission1Complete;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player_Nobile")
        {
            audioSource.clip = Resources.Load<AudioClip>("Talking/Nobile_Mercante1");
            audioSource.Play();
        }
    }

    public void Talk(GameObject caller)
    {
        if (!audioSource.isPlaying)
        {
            var index = 1;
            audioSource.clip = Resources.Load<AudioClip>("Talking/Amico" + index);
            audioSource.Play();
            //StartCoroutine(Subtitles());
            if (Mission1Complete != null && index == 0)
            {
                Mission1Complete();
                index = 1;
            }
        }
    }
}
