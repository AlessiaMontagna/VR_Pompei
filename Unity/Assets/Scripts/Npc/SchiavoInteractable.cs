using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchiavoInteractable : Interattivo
{
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;
    public bool pointingSchiavo = false; 
    public bool hasTalked = false;
    private Text _sottotitoli;
    private Animator _animator;
    private AudioSource _audioSource;
    private int index = 0;
    private AudioSubManager _subManager;

    private void Start()
    {
        _subManager = FindObjectOfType<AudioSubManager>();
        _sottotitoli = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    public override void Interact(GameObject caller)
    {
        hasTalked = true;
        UITextOff();
<<<<<<< HEAD
        Talk(Resources.Load<AudioClip>("Talking/TutorialSchiavo/" + index));
=======
        Talk(Resources.Load<AudioClip>("Talking/TutorialSchiavo/" + Globals.player.ToString()));
>>>>>>> origin/Alessia_New
    }
    public void Talk(AudioClip audio)
    {
        _audioSource.clip = audio;
        if (!Globals.someoneIsTalking)
        {
            _audioSource.Play();
            StartCoroutine(StartDialogue(audio));
        }

    }

    private IEnumerator StartDialogue(AudioClip audio)
    {
        _sottotitoli.text = _subManager.GetSubs(index, Characters.SchiavoTutorial);
        Globals.someoneIsTalking = true;
        yield return new WaitForSeconds(audio.length);
        Globals.someoneIsTalking = false;
        _sottotitoli.text ="";
<<<<<<< HEAD
=======
        yield return new WaitForSeconds(2f);
        //TODO: schiavo se ne deve andare via. 
>>>>>>> origin/Alessia_New
    }
    public override void UITextOn()
    {
        if (hasTalked) return;
        _eButton.enabled = true;
        talk.enabled = true;
        pointingSchiavo = true;
    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        talk.enabled = false;
    }

}
