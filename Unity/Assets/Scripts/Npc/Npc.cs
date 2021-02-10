using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NpcInteractable))]

public class Npc : MonoBehaviour
{
    private NavAgent _navAgent;
    private Animator _animator;
    private Characters _character;
    private GameObject _parent;
    private AudioSource _audioSource;
    private int _audioFilesCount = 0;
    private string _audioVoice;

    void Awake()
    {
        //_navAgent = new NavAgent(this);
        _animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        var _audioFiles = FindObjectOfType<AudioSubManager>().GetAudios(_character);
        _audioFilesCount = _audioFiles.Count/3;
        _audioVoice = _audioFiles.ElementAt(0);
    }

    void Update() => _navAgent?.Tik();

    public void SetCharacter(Characters c) => _character = c;

    public Characters GetCharacter(){return _character;}

    public void SetParent(GameObject parent){_parent = parent;gameObject.transform.parent = _parent.transform;}

    public GameObject GetParent(){return _parent;}

    public void SetState(string statename) => _navAgent.SetInitialState(statename);

    public void SetTargets(List<Vector3> targets) => _navAgent.SetTargets(targets);

    public void Interact() => _navAgent.interaction = true;

    public void StartInteraction() => StartCoroutine(TalkInteraction());

    private IEnumerator TalkInteraction()
    {
        Globals.someoneIsTalking = true;
        _animator.SetBool("Talk", true);
        int index = Random.Range(0, _audioFilesCount);
        _audioSource.clip = Resources.Load<AudioClip>($"Talking/{_character.ToString()}/{Globals.player.ToString()}{index}_{_audioVoice}");
        _audioSource.Play();
        FindObjectOfType<sottotitoli>().GetComponent<Text>().text = FindObjectOfType<AudioSubManager>().GetSubs(index, _character);
        yield return new WaitForSeconds(_audioSource.clip.length);
        StopInteraction();
    }

    public void UpdateInteraction()
    {
        _navAgent.CheckPlayerPosition();
        if(_animator.GetBool("Talk") && !_animator.GetBool("Turn")) _animator.SetFloat("TalkFloat", Random.Range(0f, 1f));
    }

    public void StopInteraction()
    {
        StopCoroutine(TalkInteraction());
        if(_audioSource.isPlaying)_audioSource.Stop();
        FindObjectOfType<sottotitoli>().GetComponent<Text>().text = "";
        _animator.SetBool("Talk", false);
        _navAgent.interaction = false;
        Globals.someoneIsTalking = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        _animator.SetTrigger("Hit");
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.ResetTrigger("Hit");
    }
}
