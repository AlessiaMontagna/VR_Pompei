using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(OcclusionInteract))]

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
    private OcclusionInteract _fmodAudioSource;
    public OcclusionInteract fmodAudioSource => _fmodAudioSource;
    private string _voice;
    public string voice => _voice;
    private int _audioFilesCount;
    public int audioFilesCount => _audioFilesCount;
    private Text _talk;
    private RawImage _eButton;
    private int _talkIndex;
    public int talkIndex => _talkIndex;

    public void Initialize(Characters character, GameObject parent, string statename, List<Vector3> targets)
    {
        _character = character;
        _parent = parent;
        _navAgent = new NavAgent(this);
        _navAgent.SetInitialState(statename);
        _navAgent.SetTargets(targets);
        _animator = gameObject.GetComponent<Animator>();

        _fmodAudioSource = gameObject.GetComponent<OcclusionInteract>();
        _fmodAudioSource.enabled = false;
        _fmodAudioSource.PlayerOcclusionWidening = 0.15f;
        _fmodAudioSource.SoundOcclusionWidening = 0.8f;
        LayerMask mask = LayerMask.GetMask("ProjectCameraLayer");
        _fmodAudioSource.OcclusionLayer = mask;

        _voice = GameObject.FindObjectOfType<AudioSubManager>().GetVoice(_character);
        if(_voice == null)Debug.LogError($"GetVoice({_character}) returned null");
        _audioFilesCount = 0;
        while(GameObject.FindObjectOfType<AudioSubManager>().GetAudio(_audioFilesCount, _character, _voice) != null){_audioFilesCount++;}
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        _talk = FindObjectOfType<talk>().GetComponent<Text>();
        _talkIndex = -1;
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
        if(phase > 0) StartInteraction();
        else if (phase == 0) UpdateInteraction();
        else StopInteraction();
    }

    public void SetTalkIndex(int index)
    {
        if(index < 0) _talkIndex = Random.Range(0, _audioFilesCount);
        else _talkIndex = index;
    }

    public void SetSubtitles(int index)
    {
        if(index < 0) GameObject.FindObjectOfType<sottotitoli>().GetComponent<Text>().text = "";
        else GameObject.FindObjectOfType<sottotitoli>().GetComponent<Text>().text = FindObjectOfType<AudioSubManager>().GetSubs(index, _character);
    }

    public void SetAudio(int index)
    {
        if(index < 0){_fmodAudioSource.enabled = false;return;}
        _fmodAudioSource.SelectAudio = "event:/"+ GameObject.FindObjectOfType<AudioSubManager>().GetAudio(index, _character, _voice);
        _fmodAudioSource.enabled = true;
    }

    protected virtual void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        if(_talkIndex < 0) SetTalkIndex(-1);
        StartCoroutine(Talk(_talkIndex));
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
        StopCoroutine(Talk(_talkIndex));
        _talkIndex = -1;
        SetAudio(_talkIndex);
        SetSubtitles(_talkIndex);
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), false);
        _navAgent.interaction = false;
        Globals.someoneIsTalking = false;
        _fmodAudioSource.enabled = false;
    }

    protected virtual IEnumerator Talk(int index)
    {
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        SetAudio(index);
        SetSubtitles(index);

        float length = 0;
        FMOD.RESULT res = _fmodAudioSource.AudioDes.getLength(out int fmodLength);
        if(res == FMOD.RESULT.OK) length = fmodLength /1000;

        Debug.Log($"{length}");
        yield return new WaitForSeconds(length);
        Debug.Log($"{length}");

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