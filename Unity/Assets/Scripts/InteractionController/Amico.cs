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
    private AudioSubManager _subtitles;
    private Text dialogueText;
    private void Start()
    {
        dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        _subtitles = FindObjectOfType<AudioSubManager>();
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
        Globals.someoneIsTalking = true;
        if (Globals.player == Players.Schiavo)
        {
            index = UnityEngine.Random.Range(0, 3);
        }
        audioSource.clip = Resources.Load<AudioClip>("Talking/" + Globals.player.ToString() + "_Amico" + index);
        audioSource.Play();
        if (Globals.player == Players.Nobile && Mission1Complete != null && index == 0)
        {
            Mission1Complete();
            index = 1;
        }
        StartCoroutine(Subtitles());
    }

    private IEnumerator Subtitles()
    {
        dialogueText.text = _subtitles.GetSubs(index, Characters.Amico);
        yield return new WaitForSeconds(audioSource.clip.length);
        dialogueText.text = "";
        Globals.someoneIsTalking = false;

    }
}
