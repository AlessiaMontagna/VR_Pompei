using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
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
    private bool _nearExplosion = false;

    void Awake()
    {
        _navAgent = new NavAgent(this);
        _animator = gameObject.GetComponent<Animator>();
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "ScenaFinale")
        {
            _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
            _talk = FindObjectOfType<talk>().GetComponent<TextMeshProUGUI>();
            if(Globals.language == "it")_talk.text = "Parla";
            else _talk.text = "Talk";
            _talk.enabled = false;
        }
        _audioSubManager = GameObject.FindObjectOfType<AudioSubManager>();
        _fmodAudioSource = gameObject.GetComponent<OcclusionInteract>();
        _fmodAudioSource.enabled = false;
        _fmodAudioSource.PlayerOcclusionWidening = 0.3f;
        _fmodAudioSource.SoundOcclusionWidening = 0.8f;
        LayerMask mask = LayerMask.GetMask("ProjectCameraLayer");
        _fmodAudioSource.OcclusionLayer = mask;

        // new states
        State earthquake = _navAgent.AddState(NavAgent.NavAgentStates.Earthquake.ToString(), () => { _navAgent.navMeshAgent.isStopped = true; StartCoroutine(Earthquake()); }, () => { _animator.SetBool(NavAgent.NavAgentStates.Turn.ToString(), false); _animator.SetFloat(NavAgent.NavAgentStates.Move.ToString() + _navAgent.animatorVariable, _navAgent.navMeshAgent.velocity.magnitude); if(_navAgent.navMeshAgent.destination != gameObject.transform.position && _navAgent.DestinationReached())Destroy(gameObject); }, () => {});
        State hit = _navAgent.AddState("Hit", () => { _navAgent.navMeshAgent.isStopped = true; _animator.SetBool("Hit", true); }, () => {}, () => { _animator.SetBool("Hit", false); });
        
        // new transitions
        foreach (var statename in _navAgent.GetAllStates())_navAgent.AddTransition(_navAgent.GetState(statename), earthquake, () => Globals.earthquake);
        //_navAgent.AddTransition(earthquake, _navAgent.GetState(NavAgent.NavAgentStates.Interact.ToString()), () => _navAgent.interaction);
        foreach(var statename in _navAgent.GetAllStates())_navAgent.AddTransition(_navAgent.GetState(statename), hit, () => _nearExplosion);
        _navAgent.AddTransition(hit, _navAgent.GetState(NavAgent.NavAgentStates.Move.ToString()), () => !_nearExplosion);
    }

    public void Initialize(Characters character, GameObject parent, string statename, List<Vector3> targets)
    {
        _character = character;
        _parent = parent;
        _navAgent.SetInitialState(statename);
        _navAgent.SetTargets(targets);
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "ScenaFinale")
        {
            _voice = _audioSubManager.GetVoice(character);
            if(_voice == null)Debug.LogError($"GetVoice({character}) returned null");
        }
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
        if(_eButton!= null && _talk != null)
        {
            _eButton.enabled = false;
            _talk.enabled = false;
        }
    }

    public void Interactable()
    {
        if(Globals.earthquake)Interaction(1);
        else _navAgent.interaction = true;
    }

    public void Interaction(int phase)
    {
        if(phase > 0) StartInteraction();
        else if (phase == 0) UpdateInteraction();
        else StopInteraction();
    }

    public bool ChangeVoice(string voice)
    {
        if(_audioSubManager.GetMaxAudios(_character, voice) <= 0)return false;
        _voice = voice;
        return true;
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
    
    public IEnumerator WaitToGetUp(float distance, float angle)
    {
        Debug.Log($"HIT: distance: {distance}m; angle: {angle}°");
        float delay = 1f;
        yield return new WaitForSeconds(delay);
        delay = _animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")?_animator.GetCurrentAnimatorStateInfo(0).length - delay : Random.Range(1.8f,3.3f);
        //Debug.Log($"{_navAgent.GetCurrentState().Name} ANIMATION: {delay}s");
        yield return new WaitForSeconds(delay + _animator.GetFloat("HitAngle") == 0 ? 2f : 0f);
        _nearExplosion = false;
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
        if(!Globals.earthquake)_animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "ScenaLapilli") yield break;
        _navAgent.navMeshAgent.ResetPath();
        if(!GameObject.FindObjectOfType<NavSpawner>().navspawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("SPAWN ERROR");
        NavMeshPath path = new NavMeshPath();
        _navAgent.navMeshAgent.CalculatePath(spawns.ElementAt(Random.Range(0, spawns.Count)).transform.position, path);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Earthquake"));
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        //yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Move")); // non so perché non posso mettere waituntil per controllare lo stato dell'animator
        yield return new WaitUntil(() => path.status != NavMeshPathStatus.PathPartial);
        if(!_navAgent.navMeshAgent.SetPath(path)) Destroy(gameObject);
        _animator.SetBool(NavAgent.NavAgentStates.Move.ToString(), true);
        _animator.SetBool(NavAgent.NavAgentStates.Earthquake.ToString(), false);
        _navAgent.navMeshAgent.speed = _navAgent.runSpeed;
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if(_animator.GetBool("Hit") || collider.tag != "Lapillus" || collider.GetType() != typeof(SphereCollider))return;
        float distance = Vector3.Distance(new Vector3(collider.gameObject.transform.position.x, gameObject.transform.position.y, collider.gameObject.transform.position.z), gameObject.transform.position);
        if(distance < 1f)Destroy(gameObject);
        else if(distance < 3f)Destroy(gameObject, 10f);
        _animator.SetFloat("HitDistance", distance);
        float angle = Vector3.SignedAngle((collider.gameObject.transform.position - gameObject.transform.position), gameObject.transform.forward, Vector3.up);
        if(angle > -30 && angle < 30)angle = 0;
        else if(angle > 30 && angle < 150)angle = 90;
        else if(angle > -150 && angle < -30)angle = -90;
        else if(angle > -150 && angle < 150)angle = 180;
        _animator.SetFloat("HitAngle", angle);
        _nearExplosion = true;
        StartCoroutine(WaitToGetUp(distance, angle));
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