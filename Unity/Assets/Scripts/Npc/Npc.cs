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
    private bool _turning = false;

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
    }

    void Update()
    {
        _animator.SetFloat("MoveSpeed", gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.magnitude);
        _navAgent.Tik();
    }

    public void SetCharacter(Characters c) => _character = c;

    public Characters GetCharacter(){return _character;}

    public void SetState(string statename) => _navAgent.SetState(statename);

    public void SetTargets(List<Vector3> targets) => _navAgent.SetTargets(targets);

    public void Interact() => _navAgent.Interact(true);

    public void Interaction()
    {
        Globals.someoneIsTalking = true;
        StartCoroutine(Talk());
    }

    private IEnumerator TurnToPlayer(bool b)
    {
        float angle = 0f;
        if(b) angle = Vector3.SignedAngle((GameObject.FindObjectOfType<InteractionManager>().gameObject.transform.position - gameObject.transform.position).normalized, gameObject.transform.forward, Vector3.up);
        else angle = Vector3.SignedAngle((gameObject.transform.parent.position - gameObject.transform.position).normalized, gameObject.transform.forward, Vector3.up);
        if(angle > -10f && angle < 10f){Debug.Log("Forward");_animator.SetBool("Turn", false);_animator.SetFloat("TurnDirection", 0f);}
        else {_animator.SetBool("Turn", true);_animator.SetFloat("TurnDirection", angle);}
        if(_animator.GetBool("Turn") && !_turning)
        {
            _turning = true;
            yield return new WaitForSeconds(_animator.GetNextAnimatorStateInfo(0).length);
            _turning = false;
            _animator.SetBool("Turn", false);
            _animator.SetFloat("TurnDirection", 0f);
        }
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
        _animator.SetBool("Talk", false);
        StartCoroutine(TurnToPlayer(false));
        _navAgent.Interact(false);
        Globals.someoneIsTalking = false;
    }

    public void Animate()
    {
        var index = Random.Range(0, 15);
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Interact"))_animator.SetBool("Interact", false);
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Talking"))
        {
            StartCoroutine(TurnToPlayer(true));
            while(index == _animator.GetInteger("TalkIndex")){index = Random.Range(0, 15);}
            _animator.SetInteger("TalkIndex", index);
        }
    }
}
