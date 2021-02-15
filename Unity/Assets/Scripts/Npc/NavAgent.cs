﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavAgent
{
    private NpcInteractable _owner;
    private Animator _animator;
    private FiniteStateMachine<NpcInteractable> _stateMachine;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    public bool interaction = false;

    public readonly float walkSpeed = 1.25f;
    public readonly float runSpeed = 2.5f;
    public readonly float distanceToStop = 2f;
    public readonly float forwardAngle = 30f;
    public readonly float maxInteractionDistance = 10f;

    private Dictionary<string, State> _states = new Dictionary<string, State>();
    private List<Vector3> _targets = new List<Vector3>();
    private Vector3 _lookTarget = default(Vector3);

    public enum NavAgentStates{Idle, Path, Move, Talk, Interact, Turn};
    public readonly string animatorVariable = "Float";

    public NavAgent(NpcInteractable owner)
    {
        _owner = owner;
        _stateMachine = new FiniteStateMachine<NpcInteractable>(_owner);
        _animator = _owner.GetComponent<Animator>();
        _navMeshAgent = _owner.GetComponent<UnityEngine.AI.NavMeshAgent>();
        var collider = _owner.gameObject.AddComponent<CapsuleCollider>();
        collider.center = new Vector3(0, 1f, 0);
        collider.height = 1.25f;
        collider.radius = 0.3f;
        var trigger = _owner.gameObject.AddComponent<CapsuleCollider>();
        trigger.center = new Vector3(0, 0.85f, 0);
        trigger.height = 1.7f;
        trigger.radius = 0.4f;
        trigger.isTrigger = true;

        // Settings
        _navMeshAgent.angularSpeed = 1000f;
        _navMeshAgent.acceleration = 10f;
        _navMeshAgent.stoppingDistance = distanceToStop;

        // Basic states
        State idle = AddState(NavAgentStates.Idle.ToString(), () => {_navMeshAgent.isStopped = true;}, () => {Idle();}, () => {});
        State path = AddState(NavAgentStates.Path.ToString(), () => {_navMeshAgent.isStopped = false;_animator.SetBool(NavAgentStates.Move.ToString(), true);}, () => {Path();}, () => {_animator.SetBool(NavAgentStates.Move.ToString(), false);_animator.SetFloat(NavAgentStates.Move.ToString()+animatorVariable, 0f);});
        State move = AddState(NavAgentStates.Move.ToString(), () => {_navMeshAgent.isStopped = false;_animator.SetBool(NavAgentStates.Move.ToString(), true);}, () => {Move();}, () => {_animator.SetBool(NavAgentStates.Move.ToString(), false);_animator.SetFloat(NavAgentStates.Move.ToString()+animatorVariable, 0f);});
        State talk = AddState(NavAgentStates.Talk.ToString(), () => {_navMeshAgent.isStopped = true;_animator.SetBool(NavAgentStates.Talk.ToString(), true);}, () => {Talk();}, () => {_animator.SetBool(NavAgentStates.Talk.ToString(), false);});
        State play = AddState(NavAgentStates.Interact.ToString(), () => {StartInteraction();}, () => {_owner.Interaction(0);}, () => {});

        // Basic transitions
        _stateMachine.AddTransition(idle, play, () => interaction);
        _stateMachine.AddTransition(talk, play, () => interaction);
        _stateMachine.AddTransition(path, play, () => interaction);
        _stateMachine.AddTransition(move, play, () => interaction);
        _stateMachine.AddTransition(play, idle, () => false);

        // START STATE
        SetInitialState(NavAgentStates.Idle.ToString());
    }

    public void Tik(){TurnToTarget();_stateMachine?.Tik();}

    public State AddState(string name, System.Action enter, System.Action tik, System.Action exit)
    {
        State state = new State(name, _owner, enter, tik, exit);
        if(_states.TryGetValue(name, out var s)) s = state;
        else _states.Add(name, state);
        return state;
    }

    public void AddTransition(State from, State to, System.Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

    public List<string> GetAllStates(){return new List<string>(_states.Keys);}

    public State GetState(string statename){if(_states.TryGetValue(statename, out State state))return state;return null;}

    public State GetCurrentState(){return _stateMachine.GetCurrentState();}

    public State GetPreviousState(){return _stateMachine.GetPreviousState();}

    public void SetInitialState(string statename){if(_states.TryGetValue(statename, out State state)) _stateMachine.SetState(state);}

    public void SetTargets(List<Vector3> targets){if(targets != null)_targets = new List<Vector3>(targets);}

    public bool DestinationReached(){return _navMeshAgent.remainingDistance != Mathf.Infinity && _navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance * Random.Range(0.5f,1f) && Vector3.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) <= _navMeshAgent.stoppingDistance * Random.Range(0.5f,1f);}

    public void SetAnimation(int availableAnimations)
    {
        if(_animator.GetBool("Turn") || _animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit") || !_animator.GetCurrentAnimatorStateInfo(0).IsTag("Random"))return;
        _animator.SetFloat(GetCurrentState().ToString()+animatorVariable, (float)Random.Range(0, availableAnimations));
    }

    private void Idle()
    {
        _lookTarget = _owner.parent.transform.position;
        SetAnimation(6);
    }

    private void Move()
    {
        _animator.SetFloat(NavAgentStates.Move.ToString()+animatorVariable, _navMeshAgent.velocity.magnitude);
        _lookTarget = default(Vector3);
        if(!DestinationReached())return;
        if(_targets.Count == 0){GameObject.FindObjectOfType<NavSpawner>().DestroyedAgent(_owner.character);GameObject.Destroy(_owner);return;}
        Vector3 _destination;
        if(_targets.Count == 1)_destination = _targets.First();
        else _destination = _targets.ElementAt(Random.Range(0, _targets.Count-1));
        _targets.Remove(_destination);
        if(Random.Range(0f, 1f) < 0.2f) _navMeshAgent.speed = runSpeed;
        else _navMeshAgent.speed = walkSpeed;
        _navMeshAgent.SetDestination(_destination);
    }

    private void Path()
    {
        _animator.SetFloat(NavAgentStates.Move.ToString()+animatorVariable, _navMeshAgent.velocity.magnitude);
        _lookTarget = default(Vector3);
        if(_targets.Count == 0)Debug.LogError("UNDEFINED PATH");
        if(!DestinationReached())return;
        _navMeshAgent.speed = walkSpeed;
        //if(Random.Range(0f, 1f) < 0.2f) _navMeshAgent.speed = runSpeed;
        _navMeshAgent.SetDestination(_targets.ElementAt(Random.Range(0, _targets.Count)));
    }

    private void Talk()
    {
        _lookTarget = _owner.parent.transform.position;
        SetAnimation(10);
    }
    
    public void CheckPlayerPosition()
    {
        var player = GameObject.FindObjectOfType<InteractionManager>().gameObject.transform.position;
        if(Vector3.Distance(player, _owner.gameObject.transform.position) > maxInteractionDistance) _owner.Interaction(-1);
        else _lookTarget = player;
    }

    public void TurnToTarget()
    {
        if(_lookTarget == default(Vector3)){_animator.SetBool(NavAgentStates.Turn.ToString(), false);_animator.SetFloat(NavAgentStates.Turn.ToString()+animatorVariable, 0f);return;}
        float angle = Vector3.SignedAngle((_lookTarget - _owner.gameObject.transform.position), _owner.gameObject.transform.forward, Vector3.up);
        if(angle > -forwardAngle && angle < forwardAngle){_animator.SetBool(NavAgentStates.Turn.ToString(), false);_animator.SetFloat(NavAgentStates.Turn.ToString()+animatorVariable, 0f);}
        else {_animator.SetBool(NavAgentStates.Turn.ToString(), true);_animator.SetFloat(NavAgentStates.Turn.ToString()+animatorVariable, angle);}
    }

    private void StartInteraction()
    {
        _navMeshAgent.isStopped = true;
        _stateMachine.AddTransition(GetState(NavAgentStates.Interact.ToString()), _stateMachine.GetPreviousState(), () => !interaction);
        _owner.Interaction(1);
    }
}
