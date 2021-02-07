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

    public void Interact() => _navAgent.interaction = true;

    public void Interaction() => StartCoroutine(TalkInteraction());

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

    public void AnimationUpdate()
    {
        CheckPlayerPosition();
        if(_animator.GetBool("Talk") && !_animator.GetBool("Turn")) _animator.SetFloat("TalkIndex", Random.Range(0f, 1f));
    }

    private void CheckPlayerPosition()
    {
        var player = GameObject.FindObjectOfType<InteractionManager>().gameObject.transform.position;
        if(Vector3.Distance(player, gameObject.transform.position) > 5f){StopInteraction();return;}
        float angle = Vector3.SignedAngle((player - gameObject.transform.position), gameObject.transform.forward, Vector3.up);
        if(angle > -40f && angle < 40f){_animator.SetBool("Turn", false);_animator.SetFloat("TurnAngle", 0f);}
        else {_animator.SetBool("Turn", true);_animator.SetFloat("TurnAngle", angle);}
    }

    private void StopInteraction()
    {
        StopCoroutine(TalkInteraction());
        if(_audioSource.isPlaying)_audioSource.Stop();
        FindObjectOfType<sottotitoli>().GetComponent<Text>().text = "";
        _animator.SetBool("Talk", false);
        _navAgent.interaction = false;
        Globals.someoneIsTalking = false;
        //TurnToParent();
    }

    private void TurnToParent()
    {
        do
        {
            float angle = Vector3.SignedAngle((gameObject.transform.parent.gameObject.transform.position - gameObject.transform.position), gameObject.transform.forward*-1, Vector3.up);
            if(angle > -40f && angle < 40f){Debug.Log("Forward");_animator.SetBool("Turn", false);_animator.SetFloat("TurnAngle", 0f);}
            else {_animator.SetBool("Turn", true);_animator.SetFloat("TurnAngle", angle);}
        }while(!_animator.GetBool("Turn"));
    }
}
