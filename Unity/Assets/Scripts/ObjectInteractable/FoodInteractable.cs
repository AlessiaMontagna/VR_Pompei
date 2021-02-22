using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public enum foodType { Frutta, Pesce, Pane, NonInteractable };
[RequireComponent(typeof(AudioSource))]

public class FoodInteractable : ObjectInteractable
{
    [SerializeField] private foodType _foodType;
    [SerializeField] private TextMeshProUGUI pick;
    [SerializeField] private RawImage _eButton;
    private Color highlightColor;
    private MercatoFoodManager _foodManager;
    private TextMeshProUGUI dialogueText;
    private AudioSource _audioSource;
    private AudioClip _clip;

    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
        _clip = Resources.Load<AudioClip>("FeedbackSounds/Prendere_Cibo");
        _audioSource = FindObjectOfType<FirstPersonController>().GetComponents<AudioSource>()[1];
        _foodManager = FindObjectOfType<MercatoFoodManager>();
        pick = FindObjectOfType<Pick>().GetComponent<TextMeshProUGUI>();
        if(Globals.language == "it")
        {
            pick.text = "Prendi";
        }
        else
        {
            pick.text = "Pick";
        }
        pick.enabled = false;
        dialogueText = FindObjectOfType<sottotitoli>().GetComponent<TextMeshProUGUI>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        highlightColor = new Color(0.3962264f, 0.3962264f, 0.3962264f, 1);
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    }

    public override void Interact()
    {
        if(_foodType == foodType.NonInteractable)
        {
            StartCoroutine(startDialogue("Non devo prendere queste cose"));
            return;
        }
        if (_foodManager.getCounter(_foodType) < _foodManager.getMax(_foodType))
        {
            UITextOff();
            _audioSource.clip = _clip;
            _audioSource.Play();
            _foodManager.addCounter(_foodType);
            _foodManager.CheckMissionComplete();
            _foodManager.CheckGetMaxFood(_foodType);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(startDialogue("Ho finito di prendere questo cibo."));
            //TODO: Play audio tizio
        }

    }

    public override void UITextOff()
    {
        if(gameObject != null)
        {
            _eButton.enabled = false;
            pick.enabled = false;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }
    }

    public override void UITextOn()
    {
        _eButton.enabled = true;
        pick.enabled = true;
        if (_foodManager.getCounter(_foodType) < _foodManager.getMax(_foodType) && _foodType != foodType.NonInteractable)
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", highlightColor);
        }

    }

    private IEnumerator startDialogue(string text)
    {
        dialogueText.text = text;
        yield return new WaitForSeconds(2);
        if (dialogueText.text == text)
            dialogueText.text = "";
    }
}
