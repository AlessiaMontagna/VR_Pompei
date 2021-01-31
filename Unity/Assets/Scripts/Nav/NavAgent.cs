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
        State destroy = AddState("Destroy", () => {DestroyOwner();}, () => {}, () => {});
        State idle = AddState("Idle", () => {_navMeshAgent.isStopped = true;}, () => {/*TODO:*/}, () => {});
        State talk = AddState("Talk", () => {_navMeshAgent.isStopped = true;}, () => {/*TODO:*/}, () => {});
        State path = AddState("Path", () => {_navMeshAgent.isStopped = false;}, () => {NextDestination();}, () => {});
        State move = AddState("Move", () => {_navMeshAgent.isStopped = false;}, () => {NextDestination();},() => {});
        State interact = AddState("Interact", () => {/*TODO:*/}, () => {/*TODO:*/}, () => {_stateMachine.ResetState();});

        // Basic transitions
        _stateMachine.AddTransition(destroy, idle, () => false);
        _stateMachine.AddTransition(idle, interact, () => false);
        _stateMachine.AddTransition(talk, interact, () => false);
        _stateMachine.AddTransition(path, interact, () => false);
        _stateMachine.AddTransition(move, destroy, () => false);
        _stateMachine.AddTransition(move, interact, () => false);
        _stateMachine.AddTransition(interact, destroy, () => false);

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
        else Debug.LogError($"Unknown state name: {statename}");
    }

    public void SetTargets(List<Vector3> targets){_targets = new List<Vector3>(targets);}

    public bool DestinationReached(){return _navMeshAgent.remainingDistance != Mathf.Infinity && _navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance * Random.Range(0.5f,1f);}
    
    private void NextDestination()
    {
        if(!DestinationReached() || _targets.Count == 0) return;
        Vector3 _destination;
        if(_targets.Count == 1)_destination = _targets.First();
        else _destination = _targets.ElementAt(Random.Range(0, _targets.Count-1));
        _targets.Remove(_destination);
        if(Random.Range(0f, 1f) < 0.2f) _navMeshAgent.speed = runSpeed;
        else _navMeshAgent.speed = walkSpeed;
        _navMeshAgent.SetDestination(_destination);
    }

    private void DestroyOwner()
    {
        GameObject.FindObjectOfType<NavSpawner>().DestroyedAgent(_owner.GetComponent<MonoBehaviour>().GetType().ToString());
        GameObject.Destroy(_owner);
    }
}
