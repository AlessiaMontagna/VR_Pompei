﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NpcInteractable))]

public class Npc : MonoBehaviour
{
    private NavAgent _navAgent;
    private Animator _animator;
    private Characters _character;
    private AudioSource _audioSource;
    private int _audioFilesCount = 0;
    private string _audioVoice;
    private Quaternion _defaultRotation;

    void Awake()
    {
        _navAgent = new NavAgent(this);
        _animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        var _audioFiles = FindObjectOfType<AudioSubManager>().GetAudios(_character);
        _audioFilesCount = _audioFiles.Count/3;
        _audioVoice = _audioFiles.ElementAt(0);
        var collider = GetComponent<CapsuleCollider>();
        if(collider.center.y != 0.85f)collider.center = new Vector3(collider.center.x, 0.85f, collider.center.z);
        if(collider.height < 1.6f || collider.height > 1.8f)collider.height = 1.7f;
        if(collider.radius != 1f)collider.radius = 1f;
    }

    void Update()
    {
        //if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.walkSpeed)_animator.SetBool("Run", false);
        //else if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.runSpeed)_animator.SetBool("Run", true);
        _navAgent.Tik();
    }

    public void SetCharacter(Characters c) => _character = c;

    public Characters GetCharacter(){return _character;}

    public void SetRotation(Quaternion rotation){_defaultRotation = rotation;}

    public void SetState(string statename) => _navAgent.SetState(statename);

    public void SetTargets(List<Vector3> targets) => _navAgent.SetTargets(targets);

    public void Interact() => _navAgent.Interact(true);

    public void Interaction()
    {
        Globals.someoneIsTalking = true;
        var playerTransform = GameObject.FindObjectOfType<InteractionManager>().gameObject.transform;
        if(playerTransform.rotation.eulerAngles.y > gameObject.transform.rotation.eulerAngles.y) Turn(1f);
        else if(playerTransform.rotation.eulerAngles.y < gameObject.transform.rotation.eulerAngles.y) Turn(-1f);
        gameObject.transform.LookAt(playerTransform, Vector3.up);
        Talk();
    }

    private void Turn(float turnSpeed)
    {
        _animator.SetBool("Turn", true);
        _animator.SetFloat("turnSpeed", turnSpeed);
    }

    public void Talk()
    {
        _animator.SetBool("Talk", true);
        StartCoroutine(Subtitles());
    }

    private IEnumerator Subtitles()
    {
        int index = Random.Range(0, _audioFilesCount);
        _audioSource.clip = Resources.Load<AudioClip>($"Talking/{_character.ToString()}/{Globals.player.ToString()}{index}_{_audioVoice}");
        _audioSource.Play();
        var sub = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        sub.text = FindObjectOfType<AudioSubManager>().GetSubs(index, _character);
        //Debug.Log($"Playing {_character.ToString()}/{Globals.player.ToString()}{index}_{_audioVoice}: index of {_audioFilesCount}");
        yield return new WaitForSeconds(_audioSource.clip.length);
        Globals.someoneIsTalking = false;
        _animator.SetBool("Talk", false);
        sub.text = "";
        gameObject.transform.rotation = _defaultRotation;
        _navAgent.Interact(false);
        _animator.SetBool("Turn", false);
    }

    public void Animate()
    {
        var index = Random.Range(0, 15);
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Turning"))_animator.SetBool("Turn", false);
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Talking"))while(index == _animator.GetInteger("TalkIndex")){index = Random.Range(0, 15);}
        _animator.SetInteger("TalkIndex", index);
    }
}
