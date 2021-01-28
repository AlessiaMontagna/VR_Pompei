/*
    This instanciates a NavAgent object to use as base Agent for NPCs
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent<T>
{
    private GameObject _owner;
    private FiniteStateMachine<T> _stateMachine;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;

    public readonly float walkSpeed = 1.5f;
    public readonly float runSpeed = 3f;
    public readonly float distanceToStop = 3f;

    private Dictionary<string, State> _states = new Dictionary<string, State>();

    private Vector3 _destination = default(Vector3);
    private bool _run;
    private bool _destroy = false;

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
        State move = AddState("Move", () => {}, () => {GoToDestination();}, () => {});
        State stop = AddState("Stop", () => {_navMeshAgent.isStopped = true;}, () => {if(DestinationIsValid() && _destroy)DestroyOwner();else _destination = default(Vector3);}, () => {_navMeshAgent.isStopped = false;});

        // Basic transitions
        _stateMachine.AddTransition(move, stop, () => StopAgent());
        _stateMachine.AddTransition(stop, move, () => !StopAgent());

        // START STATE
        SetState("Stop");
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

    public List<string> GetStates(){return new List<string>(_states.Keys);}

    public State GetState(string name){if(_states.TryGetValue(name, out State state))return state;return null;}

    public void SetState(string statename)
    {
        if(_states.TryGetValue(statename, out State state)) _stateMachine.SetState(state);
        else Debug.LogError($"Wrong state setup: {statename}");
    }

    public void ResetState() => _stateMachine.ResetState();

    public bool StopAgent(){return Vector3.Distance(_destination, _navMeshAgent.transform.position) <= distanceToStop;}

    public void SetDestination(Vector3 target, bool run, bool destroy)
    {
        if(!UnityEngine.AI.NavMesh.SamplePosition(target, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas)) return;
        if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _navMeshAgent.velocity.sqrMagnitude <= 0f)
        {
            _destination = target;
            _run = run;
            _destroy = destroy;
        }
    }

    public bool DestinationIsValid(){return !_destination.Equals(default(Vector3));}

    public void GoToDestination()
    {
        if(!DestinationIsValid()) return;
        //Debug.Log($"Moving to {_destination}");
        if(_run) _navMeshAgent.speed = runSpeed;
        else _navMeshAgent.speed = walkSpeed;
        _navMeshAgent.SetDestination(_destination);
    }

    private void DestroyOwner()
    {
        GameObject.FindObjectOfType<NavSpawner>().DestroyedAgent(_owner.GetComponent<MonoBehaviour>().GetType().ToString());
        GameObject.Destroy(_owner);
    }
}
