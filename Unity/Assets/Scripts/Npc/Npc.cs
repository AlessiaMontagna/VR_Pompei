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
    private string _voce;
    private int _nAudioFiles = -1;
    
    private List<string> _vociMaschili = new List<string>{"Giorgio", "Francesco", "Antonio", "Klajdi", "Edoardo", "Fabrizio", "Andrea", "Diego", "Marco", "Alessandro"};
    private List<string> _vociFemminili = new List<string>{"Alessia", "Sofia", "Nadia", "Paola", "Stella", "Camilla"};
    /*
    Giorgio: Nobile_SchiavoN_; Nobile_MercanteN_; Nobile_GuardiaN_; Nobile_NobileMN_; Mercante_GuardiaN_; Schiavo_MercanteN_; 
    Francesco: Nobile_NobileMN_; Mercante_SchiavoN_; Mercante_MercanteN_; Nobile_SchiavoN_; Mercante_NobileMN_; Schiavo_GuardiaN_; 
    Antonio: Mercante_GuardiaN_; Mercante_NobileMN_; Schiavo_SchiavoN_; Nobile_MercanteN_; Mercante_SchiavoN_; Schiavo_NobileMN_; 
    Klajdi: Schiavo_MercanteN_; Schiavo_GuardiaN_; Schiavo_NobileMN_; Nobile_GuardiaN_; Mercante_MercanteN_; Schiavo_SchiavoN_; 
    
    Edoardo: Nobile_SchiavoN_; Nobile_MercanteN_; 
    Fabrizio: Nobile_GuardiaN_; Nobile_NobileMN_; 
    Andrea: Mercante_SchiavoN_; Mercante_MercanteN_; 
    Diego: Mercante_GuardiaN_; Mercante_NobileMN_; 
    Marco: Schiavo_SchiavoN_; Schiavo_MercanteN_; 
    Alessandro: Schiavo_GuardiaN_; Schiavo_NobileMN_; 

    Alessia: Nobile_NobileFN_; Schiavo_NobileFN_;
    Sofia: Mercante_NobileFN_; Nobile_NobileFN_;
    Nadia: Schiavo_NobileFN_; Mercante_NobileFN_;
    Paola: Nobile_NobileFN_; Schiavo_NobileFN_;
    Stella: Mercante_NobileFN_; Nobile_NobileFN_;
    Camilla: Schiavo_NobileFN_; Mercante_NobileFN_;
    */
    void Awake()
    {
        _navAgent = new NavAgent(this);
        _animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        var files = Resources.LoadAll<AudioClip>("Talking").Where(i => i.name.Contains(Globals.player.ToString() + "_" + _character.ToString())).ToList();
        do
        {
            if(_character == Characters.NobileF)_voce = _vociFemminili.ElementAt(Random.Range(0, _vociFemminili.Count));
            else _voce = _vociMaschili.ElementAt(Random.Range(0, _vociMaschili.Count));
            _nAudioFiles = files.Where(i => i.name.Contains(_voce)).ToList().Count;
        }while(_nAudioFiles <= 0);
        //Debug.Log($"Found {_nAudioFiles} audios for {Globals.player.ToString() + "_" + _character.ToString()} + {_voce}");
        var collider = GetComponent<CapsuleCollider>();
        if(collider.center.y < 0.8f)collider.center = new Vector3(collider.center.x, 0.85f, collider.center.z);
        if(collider.height < 1.6f || collider.height > 1.8f)collider.height = 1.7f;
        // here add/override states and transitions();
        /*
        State interact = _navAgent.AddState("Interact", () => {}, () => {}, () => {});
        _navAgent.AddTransition(_navAgent.GetState("Talk"), interact, () => Random.Range(0f, 1f) < 0.1f);
        _navAgent.AddTransition(interact, _navAgent.GetState("Talk"), () => Random.Range(0f, 1f) < 0.6f);
        */
    }

    void Update()
    {
        //if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.walkSpeed)_animator.SetBool("Run", false);
        //else if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.runSpeed)_animator.SetBool("Run", true);
        _navAgent.Tik();
    }

    public void SetCharacter(Characters c) => _character = c;

    public Characters GetCharacter(){return _character;}

    public void SetState(string statename) => _navAgent.SetState(statename);

    public void SetTargets(List<Vector3> targets) => _navAgent.SetTargets(targets);

    public void Interact() => _navAgent.Interactive(true);

    public void StartTalking()
    {
        Debug.Log($"available audios: {_nAudioFiles}");
        if(_audioSource.isPlaying || _nAudioFiles <= 0) return;
        int index = Random.Range(1, _nAudioFiles);
        _audioSource.clip = Resources.Load<AudioClip>("Talking/" + Globals.player.ToString() + "_" + _character.ToString() + index + "_" + _voce);
        _audioSource.Play();
        StartCoroutine(Subtitles(index));
    }

    private IEnumerator Subtitles(int i)
    {
        Globals.someoneIsTalking = true;
        FindObjectOfType<sottotitoli>().GetComponent<Text>().text = FindObjectOfType<AudioSubManager>().GetSubs(i, _character);
        yield return new WaitForSeconds(_audioSource.clip.length);
        Globals.someoneIsTalking = false;
        _navAgent.Interactive(false);
        FindObjectOfType<sottotitoli>().GetComponent<Text>().text = "";
    }
}