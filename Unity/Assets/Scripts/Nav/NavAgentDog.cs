using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class NavAgentDog : MonoBehaviour
{
    private NavAgent<NavAgentDog> _navAgent;
    private Animator _animator;

    private List<Vector3> _targets = new List<Vector3>();
    protected enum Behaviours{Idle, Talk, Move, Path, Interact}
    private Behaviours behaviour = Behaviours.Idle;

    void Start()
    {
        _navAgent = new NavAgent<NavAgentDog>(gameObject, new FiniteStateMachine<NavAgentDog>(gameObject));
        _animator = gameObject.GetComponent<Animator>();
        AddStatesAndTransitions();
    }

    void Update()
    {
        List<bool> bools = new List<bool>{false, true, false, false};
        bool b = bools[Random.Range(0, bools.Count)];
        switch (behaviour)
        {
            case Behaviours.Idle: // idle
                break;
            case Behaviours.Talk:
                break;
            case Behaviours.Move: // reaches a position and destroys itself
                if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.walkSpeed)_animator.SetBool("Run", false);
                else if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.runSpeed)_animator.SetBool("Run", true);
                _navAgent.SetDestination(_targets[Random.Range(0, _targets.Count)], b, true);
                break;
            case Behaviours.Path: // follows the path
                if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.walkSpeed)_animator.SetBool("Run", false);
                else if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.runSpeed)_animator.SetBool("Run", true);
                _navAgent.SetDestination(_targets[Random.Range(0, _targets.Count)], b, false);
                break;
            case Behaviours.Interact: // interacts with the user and goes back to the previous state
                _navAgent.ResetState();
                break;
            default: throw new System.ArgumentOutOfRangeException();
        }
        _navAgent.Tik();
    }

    private void AddStatesAndTransitions()
    {
        State interact = _navAgent.AddState("Interact", () => {}, () => {}, () => {});
        foreach (var item in _navAgent.GetStates())
        {
            State state = _navAgent.GetState(item);
            if(state != null) _navAgent.AddTransition(state, interact, () => {return false;});
        }
    }

    public void SetBehaviour(string b)
    {
        switch (b)
        {
            case "Idle":
                behaviour = Behaviours.Idle;
                break;
            case "Talk":
                behaviour = Behaviours.Talk;
                break;
            case "Move":
                behaviour = Behaviours.Move;
                break;
            case "Path":
                behaviour = Behaviours.Path;
                break;
            case "Interact":
                behaviour = Behaviours.Interact;
                break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }

    public void SetTargets(List<Vector3> targets){_targets = new List<Vector3>(targets);}
}
