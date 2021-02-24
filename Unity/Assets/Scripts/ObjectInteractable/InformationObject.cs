using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InformationObject : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI _tutorialText;
    [SerializeField] private CodexInformation _objectName;
    private Codex _codex;
    private AudioSource _audioSource;
    private AudioClip _audioClip;
    private TextMeshProUGUI _nuovaVoceText;
    private bool _getInformation = false;
    private GameObject _fps;

    private void Start()
    {
        _fps = FindObjectOfType<InteractionManager>().gameObject;
        _audioSource = FindObjectOfType<InteractionManager>().GetComponents<AudioSource>()[1];
        _audioClip = Resources.Load<AudioClip>("FeedbackSounds/Nuova_voce_codex");
        _nuovaVoceText = FindObjectOfType<CodexText>().GetComponent<TextMeshProUGUI>();
        _nuovaVoceText.text = "";
        _codex = FindObjectOfType<Codex>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_getInformation && other.gameObject == _fps && FindObjectOfType<CodexFlip>().enabled == false)
        {
            StartCoroutine(NewVoice());
            _codex.addDiscoveryId((int)_objectName);
            _getInformation = true;
            _audioSource.clip = _audioClip;
            _audioSource.Play();
            foreach (BoxCollider b in GetComponents<BoxCollider>())
            {
                b.enabled = false;
            }
        }

    }

    private IEnumerator NewVoice()
    {
        if (Globals.language == "en")
        {
            _nuovaVoceText.text = "New entry in codex: " + _objectName.ToString() + "\n\npress <sprite index= 0> for more information";
        }
        else
        {
            _nuovaVoceText.text = "Nuova nota nel codex: " + _objectName.ToString() + "\n\npremi <sprite index= 0>  per informazioni";
        }
        yield return new WaitForSeconds(5);
        if(_nuovaVoceText.text == "Nuova nota nel codex: " + _objectName.ToString() + "\n\npremi <sprite index= 0>  per informazioni" ||
             _nuovaVoceText.text == "New entry in codex: " + _objectName.ToString() + "\n\npress <sprite index= 0> for more information") _nuovaVoceText.text = "";

    }
}
