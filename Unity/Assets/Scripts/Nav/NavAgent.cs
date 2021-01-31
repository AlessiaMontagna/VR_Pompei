using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavAgent<T>
{
    private GameObject _owner;
    private FiniteStateMachine<T> _stateMachine;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;

    public readonly float walkSpeed = 1.5f;
    public readonly float runSpeed = 3f;
    public readonly float distanceToStop = 2f;

    private Dictionary<string, State> _states = new Dictionary<string, State>();

    private List<Vector3> _targets = new List<Vector3>();
    private Vector3 _destination;

    public NavAgent(GameObject owner, FiniteStateMachine<T> stateMachine)
    {
        _owner = owner;
        _stateMachine = stateMachine;
        _navMeshAgent = _owner.GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Settings
        _navMeshAgent.angularSpeed = 1000f;
        _navMeshAgent.acceleration = 10f;
        _navMeshAgent.stoppingDistance = distanceToStop;

        // Basic states
        State idle = AddState("Idle", () => {_navMeshAgent.isStopped = true;}, () => {/*TODO:*/}, () => {});
        State talk = AddState("Talk", () => {_navMeshAgent.isStopped = true;}, () => {/*TODO:*/}, () => {});
        State path = AddState("Path", () => {_navMeshAgent.isStopped = false;}, () => {if(DestinationReached())NextDestination();}, () => {});
        State move = AddState("Move", () => {_navMeshAgent.isStopped = false;}, () => {if(DestinationReached()){NextDestination();}},() => {});
        State interact = AddState("Interact", () => {/*TODO:*/}, () => {/*TODO:*/}, () => {_stateMachine.ResetState();});

        // Basic transitions
        _stateMachine.AddTransition(talk, idle, () => Random.Range(0f, 1f) < 0.6f);
        _stateMachine.AddTransition(idle, talk, () => Random.Range(0f, 1f) < 0.4f);

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

    public void SetState(string statename)
    {
        if(_states.TryGetValue(statename, out State state)) _stateMachine.SetState(state);
        else Debug.LogError($"Wrong state setup: {statename}");
    }

    public bool DestinationReached(){return _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance}

    public void NextDestination()
    {
        if(_targets.Count == 0){DestroyOwner();return;}
        if(Random.Range(0f, 1f) < 0.2f) _navMeshAgent.speed = runSpeed;
        else _navMeshAgent.speed = walkSpeed;
        _destination = _targets.ElementAt(Random.Range(0, _targets.Count));
        _targets.Remove(_destination);
        _navMeshAgent.SetDestination(_destination);
    }

    private void DestroyOwner()
    {
        GameObject.FindObjectOfType<NavSpawner>().DestroyedAgent(_owner.GetComponent<MonoBehaviour>().GetType().ToString());
        GameObject.Destroy(_owner);
    }
}
