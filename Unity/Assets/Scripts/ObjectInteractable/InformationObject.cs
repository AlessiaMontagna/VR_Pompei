using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InformationObject : ObjectInteractable
{
    [SerializeField] private CodexInformation _objectName;
    [SerializeField] private Text _nuovaVoceText;
    [SerializeField] private Text _tutorialText;

    private Codex _codex;
    private AudioSource _audioSource;
    private AudioClip _audioClip;
    
    private void Start()
    {
        _audioSource = FindObjectOfType<InteractionManager>().GetComponents<AudioSource>()[1];
        _audioClip = Resources.Load<AudioClip>("FeedbackSounds/Nuova_voce_codex");
        _nuovaVoceText = FindObjectOfType<CodexText>().GetComponent<Text>();
        _codex = FindObjectOfType<Codex>();
    }

    public override void Interact()
    {
    }

    public override void UITextOff()
    {
    }

    public override void UITextOn()
    {
        StartCoroutine(NewVoice());
        _codex.addDiscoveryId((int)_objectName);
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        foreach(BoxCollider b in GetComponents<BoxCollider>())
        {
            b.enabled = false;
        }
    
    }

    private IEnumerator NewVoice()
    {
        _nuovaVoceText.text = "Nuova nota nel codex: " + _objectName.ToString();
        yield return new WaitForSeconds(5);
        if(_nuovaVoceText.text == "Nuova nota nel codex: " + _objectName.ToString()) _nuovaVoceText.text = "";

    }
}
