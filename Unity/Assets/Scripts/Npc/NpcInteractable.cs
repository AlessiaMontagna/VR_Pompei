using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private AudioSubManager _audioSubManager;
    public AudioSubManager audioSubManager => _audioSubManager;
    private GameObject _parent;
    public GameObject parent => _parent;
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
        _fmodAudioSource = gameObject.GetComponent<OcclusionInteract>();
        _fmodAudioSource.enabled = false;
        _fmodAudioSource.PlayerOcclusionWidening = 0.3f;
        _fmodAudioSource.SoundOcclusionWidening = 0.8f;
        LayerMask mask = LayerMask.GetMask("ProjectCameraLayer");
        _fmodAudioSource.OcclusionLayer = mask;

        State earthquake = _navAgent.AddState(NavAgent.NavAgentStates.Earthquake.ToString(), () => {StartCoroutine(Earthquake());}, () => {}, () => {});
        foreach (var statename in _navAgent.GetAllStates())_navAgent.AddTransition(_navAgent.GetState(statename), earthquake, () => Globals.earthquake);
        _navAgent.AddTransition(earthquake, _navAgent.GetState(NavAgent.NavAgentStates.Move.ToString()), () => !Globals.earthquake);
        _navAgent.AddTransition(earthquake, _navAgent.GetState(NavAgent.NavAgentStates.Interact.ToString()), () => !_navAgent.interaction);
        State hit = _navAgent.AddState("Hit", () => {}, () => {}, () => {});
        foreach(var statename in _navAgent.GetAllStates())_navAgent.AddTransition(_navAgent.GetState(statename), hit, () => _animator.GetBool("Hit"));
        _navAgent.AddTransition(hit, _navAgent.GetState(NavAgent.NavAgentStates.Move.ToString()), () => !_animator.GetBool("Hit"));
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
        if(targets != null && targets?.Count > 0)
        {
            var body = gameObject.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            body.mass = 100f;
            body.drag = 0.01f;
            body.angularDrag = 0.05f;
            body.useGravity = true;
            body.interpolation = RigidbodyInterpolation.None;
            body.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
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
        _fmodAudioSource.SelectAudio = "event:/"+ _audioSubManager.GetAudio(_talkIndex, _character, _voice);
        _fmodAudioSource.enabled = true;
    }
    
    public IEnumerator WaitToGetUp(Vector3 position)
    {
        Debug.Log($"HIT: {position}");
        float distance = Vector3.Distance(position, gameObject.transform.position);
        if(distance < 1f)Destroy(gameObject);
        else if(distance < 2f)Destroy(gameObject, 5f);
        _animator.SetFloat("HitDistance", distance);
        float angle = Vector3.SignedAngle((position - gameObject.transform.position), gameObject.transform.forward, Vector3.up);
        if(angle > -20 && angle < 20)_animator.SetFloat("HitAngle", 0);
        else if(angle > 20 && angle < 160)_animator.SetFloat("HitAngle", 90);
        else if(angle > -160 && angle < -20)_animator.SetFloat("HitAngle", -90);
        else if(angle > -160 && angle < 160)_animator.SetFloat("HitAngle", 180);
        _animator.SetBool("Hit", true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _animator.SetBool("Hit", false);
    }

    public void WaitForMotion(float time) => StartCoroutine(_navAgent.WaitForMotion(time));

    protected virtual void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        StartCoroutine(Talk(_talkIndex));
    }

    protected virtual void UpdateInteraction(){_navAgent.CheckPlayerPosition();}

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
        yield return new WaitForSeconds(GetAudioLength());
        StopInteraction();
    }

    protected float GetAudioLength()
    {
        FMOD.RESULT res = _fmodAudioSource.AudioDes.getLength(out int fmodLength);
        return (float)fmodLength/1000;
    }

    protected virtual IEnumerator Earthquake()
    {
        _animator.SetBool(NavAgent.NavAgentStates.Earthquake.ToString(), true);
        if(!GameObject.FindObjectOfType<NavSpawner>().navspawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("SPAWN ERROR");
        _navAgent.SetTargets(new List<Vector3>{spawns.ElementAt(0).transform.position});
        float length = 3f; //Nervously Look Around animation duration is 6.08
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Earthquake")){length = _animator.GetCurrentAnimatorStateInfo(0).length;Debug.Log($"ANIMATOR CORRETTO!! {length}");}
        yield return new WaitForSeconds(length);
        _animator.SetBool(NavAgent.NavAgentStates.Earthquake.ToString(), false);
        _navAgent.navMeshAgent.speed = _navAgent.runSpeed;
        Globals.earthquake = false;
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if(_animator.GetBool("Hit") || collider.GetType() != typeof(SphereCollider))return;
        StartCoroutine(WaitToGetUp(collider.gameObject.transform.position));
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

    protected virtual IEnumerator Earthquake()
    {
        base.Earthquake();
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
    }
}
*/