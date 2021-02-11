using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavAgent
{
    private Npc _owner;
    private Animator _animator;
    private FiniteStateMachine<Npc> _stateMachine;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    public bool _interaction = false;

    public readonly float walkSpeed = 1.5f;
    public readonly float runSpeed = 3f;
    public readonly float distanceToStop = 2f;

    private Dictionary<string, State> _states = new Dictionary<string, State>();
    private List<Vector3> _targets = new List<Vector3>();

    public NavAgent(Npc owner)
    {
        _owner = owner;
        _stateMachine = new FiniteStateMachine<Npc>(_owner);
        _animator = _owner.GetComponent<Animator>();
        _navMeshAgent = _owner.GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Settings
        _navMeshAgent.angularSpeed = 1000f;
        _navMeshAgent.acceleration = 10f;
        _navMeshAgent.stoppingDistance = distanceToStop;

        // Basic states
        State idle = AddState("Idle", () => {_navMeshAgent.isStopped = true;_animator.SetBool("Idle", true);}, () => {}, () => {});
        State path = AddState("Path", () => {_navMeshAgent.isStopped = false;_animator.SetBool("Move", true);}, () => {_animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);NextDestinationPath();}, () => {_animator.SetBool("Move", false);});
        State move = AddState("Move", () => {_navMeshAgent.isStopped = false;_animator.SetBool("Move", true);}, () => {_animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);NextDestinationMove();}, () => {_animator.SetBool("Move", false);});
        State talk = AddState("Talk", () => {_navMeshAgent.isStopped = true;_animator.SetBool("Talk", true);}, () => {Talk();}, () => {_animator.SetBool("Talk", false);});
        State interact = AddState("Interact", () => {}, () => {}, () => {});
        interact = AddState("Interact", () => {_navMeshAgent.isStopped = true;_stateMachine.AddTransition(interact, _stateMachine.GetPreviousState(), () => !_interaction);_owner.Interaction();}, () => {_owner.Animate();}, () => {});

        // Basic transitions
        _stateMachine.AddTransition(idle, interact, () => _interaction);
        _stateMachine.AddTransition(talk, interact, () => _interaction);
        _stateMachine.AddTransition(path, interact, () => _interaction);
        _stateMachine.AddTransition(move, interact, () => _interaction);
        _stateMachine.AddTransition(interact, idle, () => false);

        // START STATE
        SetState("Idle");
    }

    public void Tik() => _stateMachine.Tik();

    public State AddState(string name, System.Action enter, System.Action tik, System.Action exit)
    {
        State state = new State(name, _owner, enter, tik, exit);
        if(_states.TryGetValue(name, out var s)) s = state;
        else _states.Add(name, state);
        return state;
    }

    public void AddTransition(State from, State to, System.Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

    public List<string> GetAllStates(){return new List<string>(_states.Keys);}

    public State GetState(string name){if(_states.TryGetValue(name, out State state))return state;return null;}

    public State GetCurrentState(){return _stateMachine.GetCurrentState();}

    public State GetPreviousState(){return _stateMachine.GetPreviousState();}

    public void SetState(string statename){if(_states.TryGetValue(statename, out State state)) _stateMachine.SetState(state);}

    public void SetTargets(List<Vector3> targets){_targets = new List<Vector3>(targets);}

    public bool DestinationReached(){return _navMeshAgent.remainingDistance != Mathf.Infinity && _navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance * Random.Range(0.5f,1f) && Vector3.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) <= _navMeshAgent.stoppingDistance * Random.Range(0.5f,1f);}
    
    public void Interact(bool i){_interaction = i;}

    private void NextDestinationMove()
    {
        if(!DestinationReached())return;
        if(_targets.Count == 0){GameObject.FindObjectOfType<NavSpawner>().DestroyedAgent(_owner.GetCharacter());GameObject.Destroy(_owner);;return;}
        Vector3 _destination;
        if(_targets.Count == 1)_destination = _targets.First();
        else _destination = _targets.ElementAt(Random.Range(0, _targets.Count-1));
        _targets.Remove(_destination);
        if(Random.Range(0f, 1f) < 0.2f) _navMeshAgent.speed = runSpeed;
        else _navMeshAgent.speed = walkSpeed;
        _navMeshAgent.SetDestination(_destination);
    }

    private void NextDestinationPath()
    {
        if(_targets.Count == 0)Debug.LogError("UNDEFINED PATH");
        if(!DestinationReached())return;
        _navMeshAgent.speed = walkSpeed;
        //if(Random.Range(0f, 1f) < 0.2f) _navMeshAgent.speed = runSpeed;
        _navMeshAgent.SetDestination(_targets.ElementAt(Random.Range(0, _targets.Count)));
    }

    private void Talk()
    {
        var index = Random.Range(0, 15);
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Talking"))
        while(index == _animator.GetInteger("TalkIndex")){index = Random.Range(0, 15);}
        _animator.SetInteger("TalkIndex", index);
    }
}
