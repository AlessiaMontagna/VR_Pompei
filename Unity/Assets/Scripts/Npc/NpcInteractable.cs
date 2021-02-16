using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(OcclusionInteract))]

public class NpcInteractable : Interattivo
{
    private NavAgent _navAgent;
    public NavAgent navAgent => _navAgent;
    private Animator _animator;
    public Animator animator => _animator;
    private Characters _character;
    public Characters character => _character;
    private AudioSubManager _audioSubManager;
    public AudioSubManager audioSubManager => _audioSubManager;
    private GameObject _parent;
    public GameObject parent => _parent;
    //private AudioSource _audioSource;
    //public AudioSource audioSource => _audioSource;
    private OcclusionInteract _fmodAudioSource;
    public OcclusionInteract fmodAudioSource => _fmodAudioSource;
    private string _voice;
    public string voice => _voice;
    private TextMeshProUGUI _talk;
    private RawImage _eButton;
    private int _talkIndex;
    public int talkIndex => _talkIndex;

    void Awake()
    {
        _navAgent = new NavAgent(this);
        _animator = gameObject.GetComponent<Animator>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        _talk = FindObjectOfType<talk>().GetComponent<TextMeshProUGUI>();
        _audioSubManager = GameObject.FindObjectOfType<AudioSubManager>();
        //_audioSource = gameObject.GetComponent<AudioSource>();
        _fmodAudioSource = gameObject.GetComponent<OcclusionInteract>();
        _fmodAudioSource.enabled = false;
        _fmodAudioSource.PlayerOcclusionWidening = 0.3f;
        _fmodAudioSource.SoundOcclusionWidening = 0.8f;
        LayerMask mask = LayerMask.GetMask("ProjectCameraLayer");
        _fmodAudioSource.OcclusionLayer = mask;

        State earthquake = _navAgent.AddState(NavAgent.NavAgentStates.Earthquake.ToString(), () => {StartCoroutine(Earthquake());}, () => {}, () => {});
        foreach (var statename in _navAgent.GetAllStates())_navAgent.AddTransition(_navAgent.GetState(statename), earthquake, () => Globals.earthquake);
        _navAgent.AddTransition(earthquake, _navAgent.GetState(NavAgent.NavAgentStates.Move.ToString()), () => _navAgent.run);
        _navAgent.AddTransition(earthquake, _navAgent.GetState(NavAgent.NavAgentStates.Interact.ToString()), () => _navAgent.interaction);
    }

    public void Initialize(Characters character, GameObject parent, string statename, List<Vector3> targets)
    {
        _character = character;
        _parent = parent;
        _navAgent.SetInitialState(statename);
        _navAgent.SetTargets(targets);
        _voice = _audioSubManager.GetVoice(character);
        if(_voice == null)Debug.LogError($"GetVoice({character}) returned null");
        _talkIndex = -1;
    }

    void Update() => _navAgent?.Tik();

    public override void Interact() => Interactable();

    public override void UITextOn()
    {
        if(_eButton == null) _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        if(_talk == null) _talk = FindObjectOfType<talk>().GetComponent<TextMeshProUGUI>();
        _eButton.enabled = true;
        _talk.enabled = true;
    }

    public override void UITextOff()
    {
        if(_eButton == null) _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        if(_talk == null) _talk = FindObjectOfType<talk>().GetComponent<TextMeshProUGUI>();
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

    public void SetSubtitles(int index)
    {
        if(index < 0) GameObject.FindObjectOfType<sottotitoli>().GetComponent<TextMeshProUGUI>().text = "";
        else GameObject.FindObjectOfType<sottotitoli>().GetComponent<TextMeshProUGUI>().text = _audioSubManager.GetSubs(index, _character);
    }

    public void SetAudio(int index)
    {
        if(index < 0) _talkIndex = Random.Range(0, _audioSubManager.GetMaxAudios(_character, _voice));
        else _talkIndex = index;
        //Debug.Log(_audioSubManager.GetAudio(_talkIndex, _character, _voice));
        _fmodAudioSource.SelectAudio = "event:/"+ _audioSubManager.GetAudio(_talkIndex, _character, _voice);
        _fmodAudioSource.enabled = true;
        //_audioSource.clip = Resources.Load<AudioClip>(_audioSubManager.GetAudio(_talkIndex, _character, _voice));
        //if(_audioSource?.clip == null){Debug.LogError($"{_audioSubManager.GetAudio(_talkIndex, _character, _voice)} NOT FOUND");StopInteraction();}
    }

    protected virtual void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        StartCoroutine(Talk(_talkIndex));
    }

    protected virtual void UpdateInteraction()
    {
        _navAgent.CheckPlayerPosition();
        _navAgent.SetAnimation(10);
    }

    protected virtual void StopInteraction()
    {
        StopCoroutine(Talk(_talkIndex));
        _talkIndex = -1;
        SetSubtitles(_talkIndex);
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), false);
        _navAgent.interaction = false;
        _fmodAudioSource.enabled = false;
        Globals.someoneIsTalking = false;
    }
    protected virtual IEnumerator Talk(int index)
    {
        _animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        SetAudio(index);
        SetSubtitles(_talkIndex);
        /*
        //yield return new WaitForSeconds(_audioSource.clip.length);
        int fmodLength;
        float length = 0;
        FMOD.RESULT res = _fmodAudioSource.AudioDes.getLength(out fmodLength);
        length = fmodLength;
        Debug.Log("waitForSecond " + length);
        yield return new WaitForSeconds(length/1000);
        */
        yield return new WaitForSeconds(GetAudioLength());
        StopInteraction();
    }

    protected float GetAudioLength()
    {
        //return _audioSource.clip.length;
        FMOD.RESULT res = _fmodAudioSource.AudioDes.getLength(out int fmodLength);
        //Debug.Log("waitForSecond " + (float)fmodLength/1000);
        return (float)fmodLength/1000;
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
        _animator.ResetTrigger("Hit");
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))return;
        _animator.SetFloat("HitFloat", 0f);
        _animator.SetFloat("HitReaction", 0f);
        _animator.SetFloat("HitNobile", 0f);
    }

    protected virtual IEnumerator Earthquake()
    {
        Debug.Log("Begin Coroutine Earthquake");
        _animator.SetFloat(NavAgent.NavAgentStates.Earthquake.ToString()+_navAgent.animatorVariable, (float)Random.Range(0, 2));
        _animator.SetBool(NavAgent.NavAgentStates.Earthquake.ToString(), true);
        if(!GameObject.FindObjectOfType<NavSpawner>().navspawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("SPAWN ERROR");
        Debug.Log($"Run to: {spawns.ElementAt(0).transform.position}");
        _navAgent.SetTargets(new List<Vector3>{spawns.ElementAt(0).transform.position});
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _animator.SetBool(NavAgent.NavAgentStates.Earthquake.ToString(), false);
        _animator.SetBool(NavAgent.NavAgentStates.Move.ToString(), true);
        _navAgent.run = true;
        Debug.Log("End Coroutine Earthquake");
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

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
    }

    protected override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);
    }
}
*/