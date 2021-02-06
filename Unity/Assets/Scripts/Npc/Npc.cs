using System.Collections;
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
        collider.center = new Vector3(collider.center.x, 0.85f, collider.center.z);
        collider.height = 1.6f;
        collider.radius = 0.6f;
        //collider.isTrigger = true;
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
        TurnToPlayer(true);
        StartCoroutine(Talk());
    }

    private void TurnToPlayer(bool player)
    {
        if(!player){gameObject.transform.rotation = _defaultRotation;return;}
        var playerTransform = GameObject.FindObjectOfType<InteractionManager>().gameObject.transform;
        if(playerTransform.rotation.eulerAngles.y > gameObject.transform.rotation.eulerAngles.y) _animator.SetFloat("turnSpeed", -1f);
        else if(playerTransform.rotation.eulerAngles.y < gameObject.transform.rotation.eulerAngles.y) _animator.SetFloat("turnSpeed", 1f);
        else {_animator.SetBool("Turn", true);return;}
        gameObject.transform.LookAt(playerTransform, Vector3.up);
        _animator.SetBool("Turn", true);
    }

    private IEnumerator Talk()
    {
        _animator.SetBool("Talk", true);
        int index = Random.Range(0, _audioFilesCount);
        _audioSource.clip = Resources.Load<AudioClip>($"Talking/{_character.ToString()}/{Globals.player.ToString()}{index}_{_audioVoice}");
        _audioSource.Play();
        var sub = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        sub.text = FindObjectOfType<AudioSubManager>().GetSubs(index, _character);
        yield return new WaitForSeconds(_audioSource.clip.length);
        sub.text = "";
        TurnToPlayer(false);
        _animator.SetBool("Talk", false);
        _navAgent.Interact(false);
        Globals.someoneIsTalking = false;
    }

    public void Animate()
    {
        var index = Random.Range(0, 15);
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Interact"))_animator.SetBool("Interact", false);
        else _animator.SetBool("Turn", false);
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Talking"))
        {
            while(index == _animator.GetInteger("TalkIndex")){index = Random.Range(0, 15);}
            _animator.SetInteger("TalkIndex", index);
        }
    }
}
