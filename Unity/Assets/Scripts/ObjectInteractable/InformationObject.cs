using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class InformationObject : ObjectInteractable
{
    [SerializeField] private CodexInformation _objectName;
    [SerializeField] private Text _nuovaVoceText;
<<<<<<< HEAD
    private Codex _codex;
    private AudioSource _audiosource;

=======
    [SerializeField] private Text _tutorialText;

    private Codex _codex;
    private AudioSource _audiosource;
    
>>>>>>> origin/Alessia_New
    private void Start()
    {
        _audiosource = GetComponent<AudioSource>();
        _audiosource.clip = Resources.Load<AudioClip>("FeedbackSounds/Nuova_voce_codex");
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
        _audiosource.Play();
        foreach(BoxCollider b in GetComponents<BoxCollider>())
        {
            b.enabled = false;
        }
<<<<<<< HEAD
=======
    
>>>>>>> origin/Alessia_New
    }

    private IEnumerator NewVoice()
    {
        _nuovaVoceText.text = "Nuova nota nel codex: " + _objectName.ToString();
        yield return new WaitForSeconds(5);
        if(_nuovaVoceText.text == "Nuova nota nel codex: " + _objectName.ToString()) _nuovaVoceText.text = "";

    }
}
