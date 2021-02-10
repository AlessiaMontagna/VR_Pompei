using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NpcInteractable))]

public class NpcSuperClass : MonoBehaviour
{
    private NavAgent _navAgent;
    public NavAgent navAgent => _navAgent;
    private Animator _animator;
    public Animator animator => _animator;
    private Characters _character;
    public Characters character => _character;
    private GameObject _parent;
    public GameObject parent => _parent;
    private AudioSource _audioSource;
    public AudioSource audioSource => _audioSource;
    private int _audioFilesCount;
    public int audioFilesCount => _audioFilesCount;
    private string _audioVoice;
    public string audioVoice => _audioVoice;

    public void Initialize(Characters character, GameObject parent, string statename, List<Vector3> targets)
    {
        _character = character;
        _parent = parent;
        _navAgent = new NavAgent(this);
        _navAgent.SetInitialState(statename);
        _navAgent.SetTargets(targets);
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        var _audioFiles = GameObject.FindObjectOfType<AudioSubManager>().GetAudios(_character);
        _audioFilesCount = _audioFiles.Count/3;
        _audioVoice = _audioFiles.ElementAt(0);
    }

    void Update() => _navAgent?.Tik();

    public void Interact() => _navAgent.interaction = true;

    public void Interaction(int phase)
    {
        switch (phase)
        {
            case 1:StartCoroutine(StartInteraction());break;
            case 0:UpdateInteraction();break;
            case -1:StopInteraction();break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }

    public void SetSubtitles(string text){GameObject.FindObjectOfType<sottotitoli>().GetComponent<Text>().text = text;}

    protected virtual IEnumerator StartInteraction()
    {
        Globals.someoneIsTalking = true;
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        int index = Random.Range(0, _audioFilesCount);
        _audioSource.clip = Resources.Load<AudioClip>($"Talking/{_character.ToString()}/{Globals.player.ToString()}{index}_{_audioVoice}");
        if(_audioSource?.clip == null){Debug.Log($"Talking/{_character.ToString()}/{Globals.player.ToString()}{index}_{_audioVoice} NOT FOUND");StopInteraction();}
        _audioSource.Play();
        SetSubtitles(FindObjectOfType<AudioSubManager>().GetSubs(index, _character));
        yield return new WaitForSeconds(_audioSource.clip.length);
        StopInteraction();
    }

    protected virtual void UpdateInteraction()
    {
        _navAgent.CheckPlayerPosition();
        if(_animator.GetBool(NavAgent.NavAgentStates.Talk.ToString()) && !_animator.GetBool("Turn")) _animator.SetFloat(NavAgent.NavAgentStates.Talk.ToString()+"Float", Random.Range(0f, 1f));
    }

    protected virtual void StopInteraction()
    {
        StopCoroutine(StartInteraction());
        if(_audioSource.isPlaying)_audioSource?.Stop();
        SetSubtitles("");
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), false);
        _navAgent.interaction = false;
        Globals.someoneIsTalking = false;
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        _animator.SetTrigger("Hit");
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        _animator.ResetTrigger("Hit");
    }
}
