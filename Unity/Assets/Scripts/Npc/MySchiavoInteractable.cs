using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CapsuleCollider))]

public class MySchiavoInteractable : Interattivo
{
    public bool _switch = false;

    [SerializeField] private GameObject fpc2;
    [SerializeField] private GameObject nobile;
    private MissionManager _mission;
    private Text talk;
    private RawImage _eButton;
    private AudioSubManager _sottotitoli;
    private AudioSource _audioSource;
    private Text _dialogueText;
    private int index = 0;
    private string path;

    void Start() 
    { 
        _mission = FindObjectOfType<MissionManager>();
        _sottotitoli = FindObjectOfType<AudioSubManager>();
        _audioSource = GetComponent<AudioSource>();
        _dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
    }
    public override void Interact()
    {
        Globals.someoneIsTalking = true;
        if (_switch)
        {
            index = 1;
            StartCoroutine(Subtitles(index));
        }
        else
        {
            _audioSource.clip = Resources.Load<AudioClip>("Talking/MySchiavo/0");
            _audioSource.Play();
            StartCoroutine(StaticDialogue(_audioSource.clip));
        }
        
        
    }

    private IEnumerator Subtitles(int index)
    {
        var player = FindObjectOfType<InteractionManager>();
        var playerAudio = player.GetComponents<AudioSource>()[1];
        FirstPersonController playerController = player.GetComponent<FirstPersonController>();
        playerController.enabled = false;
        player.GetComponent<InteractionManager>().enabled = false;
        GetComponent<MySchiavoInteractable>().UITextOff();
        path = "Talking/MySchiavo/";
        _audioSource.clip = Resources.Load<AudioClip>(path + index.ToString());
        _audioSource.Play();
        _dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds(_audioSource.clip.length);
        playerAudio.clip = Resources.Load<AudioClip>(path + index.ToString());
        playerAudio.Play();
        _dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds((playerAudio.clip.length) / 2);
        _dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds((playerAudio.clip.length) / 2);
        _dialogueText.text = "";
        //chiama animazione swoosh
        //GameObject go = Instantiate(nobile, player.transform.position, player.transform.rotation, transform) as GameObject;
        Globals.someoneIsTalking = false;
        Globals.player = Players.Schiavo;

        GetComponent<CapsuleCollider>().enabled = false;
        _mission.UpdateMission(Missions.Mission3_GetFood);
        playerController.teleporting = true;
        yield return new WaitForEndOfFrame();
        player.transform.rotation = transform.rotation;
        Vector3 newPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.position = newPosition;
        playerController.InitMouseLook(player.transform);
        yield return new WaitForFixedUpdate();
        playerController.teleporting = false;
        playerController.enabled = true;
        player.GetComponent<InteractionManager>().enabled = true;


        Destroy(gameObject);

    }

    private IEnumerator StaticDialogue(AudioClip clip)
    {
        _dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        Debug.Log(_dialogueText.text);
        yield return new WaitForSeconds(clip.length);
        _dialogueText.text = "";
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