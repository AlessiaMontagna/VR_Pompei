using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmicoInteractable : Interattivo
{
    private Text talk;
    private RawImage _eButton;
    //private AudioSource audioSource;
    private int index = 0;
    private AudioSubManager _subtitles;
    private Text dialogueText;
    private MissionManager _updateMission;
    private int _maxRand;

    [SerializeField] private OcclusionInteract _fmodAudioSource;

    /* FMOD.Studio.EventInstance playerState;

    [FMODUnity.EventRef]
    public string PlayerIntroEvent = "";
    FMOD.Studio.EventInstance playerIntro; */

    void Start()
    {   
        _fmodAudioSource = GetComponent<OcclusionInteract>();
        _updateMission = FindObjectOfType<MissionManager>();
        dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        _subtitles = FindObjectOfType<AudioSubManager>();
        //audioSource = GetComponent<AudioSource>();
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
    }

    public override void Interact()
    {
        Globals.someoneIsTalking = true;
        if (Globals.player == Players.Schiavo)
        {
            _maxRand = _subtitles.GetMaxAudios(Characters.Amico);
            index = Random.Range(0, _maxRand);
        }
        _fmodAudioSource.SelectAudio = "event:/Talking/Amico/" + Globals.player.ToString() + index;
        _fmodAudioSource.enabled = true;
        //playerIntro = FMODUnity.RuntimeManager.CreateInstance("event:/Talking/Amico/" + Globals.player.ToString() + index);
        //playerIntro.start();
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerIntro, GetComponent<Transform>(), GetComponent<Rigidbody>());
        //audioSource.clip = Resources.Load<AudioClip>("Talking/Amico/" + Globals.player.ToString() + index);
        //audioSource.Play();
        StartCoroutine(Subtitles());
        if (Globals.player == Players.Nobile && index == 0)
        {
            FindObjectOfType<MySchiavoInteractable>()._switch = true;
            _updateMission.UpdateMission(Missions.Mission2_FindSlave);
            index = 1;
        }

    }

    private IEnumerator Subtitles()
    {
        dialogueText.text = _subtitles.GetSubs(index, Characters.Amico);

        int fmodLength;
        float length = 0;

        FMOD.RESULT res = _fmodAudioSource.AudioDes.getLength(out fmodLength);

        if (res == FMOD.RESULT.OK)
            length = fmodLength /1000;
        yield return new WaitForSeconds(length);
        
        dialogueText.text = "";
        Globals.someoneIsTalking = false;
        _fmodAudioSource.enabled = false;

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