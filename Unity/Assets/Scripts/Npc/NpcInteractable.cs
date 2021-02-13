﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class NpcInteractable : Interattivo
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
    private string _voice;
    public string voice => _voice;
    private int _audioFilesCount;
    public int audioFilesCount => _audioFilesCount;
    private Text _talk;
    private RawImage _eButton;
    private int _subIndex;
    public int subIndex => _subIndex;

    public void Initialize(Characters character, GameObject parent, string statename, List<Vector3> targets)
    {
        _character = character;
        _parent = parent;
        _navAgent = new NavAgent(this);
        _navAgent.SetInitialState(statename);
        _navAgent.SetTargets(targets);
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        _voice = GameObject.FindObjectOfType<AudioSubManager>().GetVoice(_character);
        if(_voice == null)Debug.LogError($"GetVoice({_character}) returned null");
        _audioFilesCount = 0;
        while(GameObject.FindObjectOfType<AudioSubManager>().GetAudio(_audioFilesCount, _character, _voice) != null){_audioFilesCount++;}
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        _talk = FindObjectOfType<talk>().GetComponent<Text>();
    }

    void Update() => _navAgent?.Tik();

    public override void Interact() => Interactable();

    public override void UITextOn()
    {
        if(_eButton == null) _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        if(_talk == null) _talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton.enabled = true;
        _talk.enabled = true;
    }

    public override void UITextOff()
    {
        if(_eButton == null) _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        if(_talk == null) _talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton.enabled = false;
        _talk.enabled = false;
    }

    public void Interactable() => _navAgent.interaction = true;

    public void Interaction(int phase)
    {
        if(phase > 0) StartCoroutine(StartInteraction());
        else if (phase == 0) UpdateInteraction();
        else StopInteraction();
    }

    public void SetSubtitles(int index)
    {
        if(index < 0) GameObject.FindObjectOfType<sottotitoli>().GetComponent<Text>().text = "";
        else GameObject.FindObjectOfType<sottotitoli>().GetComponent<Text>().text = FindObjectOfType<AudioSubManager>().GetSubs(index, _character);
    }
    
    public void SetAudio(int index)
    {
        if(index < 0) return;
        _audioSource.clip = Resources.Load<AudioClip>(GameObject.FindObjectOfType<AudioSubManager>().GetAudio(index, _character, _voice));
        if(_audioSource?.clip == null){Debug.LogError($"{GameObject.FindObjectOfType<AudioSubManager>().GetAudio(index, _character, _voice)} NOT FOUND");StopInteraction();}
        _audioSource.Play();
    }

    protected virtual void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        _subIndex = Random.Range(0, _audioFilesCount);
        StartCoroutine(Talk(_subIndex));
    }

    protected virtual void UpdateInteraction()
    {
        _navAgent.CheckPlayerPosition();
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Talk") && !_animator.GetBool(NavAgent.NavAgentStates.Turn.ToString()))
        {
            var animation = _navAgent.talkingAnimations.ElementAt(Random.Range(0, _navAgent.talkingAnimations.Count)).ToString();
            foreach (var item in _navAgent.talkingAnimations)
            {
                var set = 0f;
                if(item.ToString() == animation) set = 1f;
                _animator.SetFloat(NavAgent.NavAgentStates.Talk.ToString()+item.ToString(), set);
            }
        }
    }

    protected virtual void StopInteraction()
    {
        StopCoroutine(Talk(_subIndex));
        if(_audioSource.isPlaying)_audioSource?.Stop();
        SetSubtitles(-1);
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), false);
        _navAgent.interaction = false;
        Globals.someoneIsTalking = false;
    }

    protected virtual IEnumerator Talk(int index)
    {
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        SetAudio(index);
        SetSubtitles(index);
        yield return new WaitForSeconds(_audioSource.clip.length);
        StopInteraction();
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if(_animator == null || collider?.tag != "Player" || _animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))return;
        _animator.SetTrigger("Hit");
        if(_animator.GetBool("Move"))
        {
            _animator?.SetFloat("MoveFloat", 1f);
            if(Globals.player == Players.Schiavo){_animator?.SetFloat("HitReaction", 1f);_animator?.SetFloat("HitNobile", 0f);}
            else {_animator?.SetFloat("HitReaction", 0f);_animator?.SetFloat("HitNobile", 1f);}
        }
        _animator.SetFloat("HitFloat", Vector3.SignedAngle((GameObject.FindObjectOfType<InteractionManager>().gameObject.transform.position - gameObject.transform.position), gameObject.transform.forward, Vector3.up));
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if(_animator == null) return;
        _animator.SetTrigger("Hit");
        _animator.ResetTrigger("Hit");
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))return;
        _animator.SetFloat("HitFloat", 0f);
        _animator.SetFloat("HitReaction", 0f);
        _animator.SetFloat("HitNobile", 0f);
    }
}

/* SUBCLASS EXAMPLE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSubClass : NpcInteractable
{
    protected override void StartInteraction()
    {
        base.StartInteraction();
    }

    protected override void UpdateInteraction()
    {
        base.UpdateInteraction();
    }

    protected override void StopInteraction()
    {
        base.StopInteraction();
    }

    protected override IEnumerator Talk(int index)
    {
        return base.Talk(index);
    }
}
*/