using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmicoInteractable : Interattivo
{
    private Text talk;
    private RawImage _eButton;
    private AudioSource audioSource;
    private int index = 0;
    private AudioSubManager _subtitles;
    private Text dialogueText;

    void Start()
    {
        dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        _subtitles = FindObjectOfType<AudioSubManager>();
        audioSource = GetComponent<AudioSource>();
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
    }

    public override void Interact(GameObject caller)
    {
        Globals.someoneIsTalking = true;
        if (Globals.player == Players.Schiavo)
        {
            index = Random.Range(0, 3);
        }
        audioSource.clip = Resources.Load<AudioClip>("Talking/Amico/" + Globals.player.ToString() + index);
        audioSource.Play();
        StartCoroutine(Subtitles());
        if (Globals.player == Players.Nobile && index == 0)
        {
            FindObjectOfType<MySchiavoInteractable>()._switch = true;
            index = 1;
        }

    }

    private IEnumerator Subtitles()
    {
        dialogueText.text = _subtitles.GetSubs(index, Characters.Amico);
        yield return new WaitForSeconds(audioSource.clip.length);
        dialogueText.text = "";
        Globals.someoneIsTalking = false;

    }

    public override void UITextOn()
    {
        _eButton.enabled = true;
        talk.enabled = true;
    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        talk.enabled = false;
    }
}