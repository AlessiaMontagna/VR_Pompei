using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class Npc : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    private NavAgent _navAgent;
    private Animator _animator;
    private Characters character;

    void Awake()
    {
        _navAgent = new NavAgent(this);
        //_animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
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

    public void SetCharacter(Characters c) => character = c;

    public Characters GetCharacter(){return character;}

    public void SetState(string statename) => _navAgent.SetState(statename);

    public void SetTargets(List<Vector3> targets) => _navAgent.SetTargets(targets);
}
