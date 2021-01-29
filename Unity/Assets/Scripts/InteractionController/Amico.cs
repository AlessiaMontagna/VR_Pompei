using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Amico : MonoBehaviour
{
    public Action Mission1Complete;
    [SerializeField] private AudioSource audioSource;
    private int index = 0;
    [SerializeField] private Text dialogueText;
    private bool _talkState;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
            audioSource.clip = Resources.Load<AudioClip>("Talking/Amico" + index);
            audioSource.Play();
            StartCoroutine(Subtitles());
            if (Mission1Complete != null && index == 0)
            {
                Mission1Complete();
                index = 1;
            }
        }

    }

    private IEnumerator Subtitles()
    {
        GetComponent<AmicoInteractable>().isTalking = true;
        dialogueText.text = "Ciao amico stasera veniamo a cena da te";
        yield return new WaitForSeconds(audioSource.clip.length);
        GetComponent<AmicoInteractable>().isTalking = false;
        dialogueText.text = "";

    }
}
