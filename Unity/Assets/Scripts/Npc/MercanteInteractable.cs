using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercanteInteractable : Interattivo
{

    [SerializeField] private MercanteFoodTypes _foodType;

    private Text talk;
    private RawImage _eButton;
    private Transform food;
    private AudioSubManager _subtitles;
    private Text _dialogueText;
    //private AudioSource audioSource;
    private bool unlockFood = true;
    private int index;

    [SerializeField] private OcclusionInteract _fmodAudioSource;

    void Start()
    {
        switch (_foodType)
        {
            case MercanteFoodTypes.Frutta:
                food = FindObjectOfType<Frutta>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Pane:
                food = FindObjectOfType<Pane>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Pesce:
                food = FindObjectOfType<Pesce>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Vasi:
                food = FindObjectOfType<Vaso>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Verdura:
                food = FindObjectOfType<Verdura>().GetComponent<Transform>();
                break;
        }

        _fmodAudioSource = GetComponent<OcclusionInteract>();
        _subtitles = FindObjectOfType<AudioSubManager>();
        _dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        //audioSource = GetComponent<AudioSource>();
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
    }
    public override void Interact(GameObject caller)
    {
        _fmodAudioSource.SelectAudio = "event:/Talking/Mercante/Mercante0_Antonio";
        _fmodAudioSource.enabled = true;
        //audioSource.clip = Resources.Load<AudioClip>("Talking/Mercante/" + Globals.player + index);
        //audioSource.Play();
        if (unlockFood && Globals.player == Players.Schiavo)
        {
            for (int i = 0; i < food.childCount; i++)
                food.GetChild(i).GetComponent<BoxCollider>().enabled = true;
            unlockFood = false;
        }
        StartCoroutine(Sottotitoli());
    }

    private IEnumerator Sottotitoli()
    {
        Globals.someoneIsTalking = true;
        _dialogueText.text = _subtitles.GetSubs(index, Characters.Mercante);
        Debug.Log(_dialogueText.text);

        int fmodLength;
        float length = 0;

        FMOD.RESULT res = _fmodAudioSource.AudioDes.getLength(out fmodLength);

        if (res == FMOD.RESULT.OK)
            length = fmodLength /1000;
        yield return new WaitForSeconds(length);
        
        _dialogueText.text = "";
        Globals.someoneIsTalking = false;
        _fmodAudioSource.enabled = false;

    }

    public override void UITextOn()
    {
        
        talk.enabled = true;
        _eButton.enabled = true;
        
    }

    public override void UITextOff()
    {
        talk.enabled = false;
        _eButton.enabled = false;
    }
}
