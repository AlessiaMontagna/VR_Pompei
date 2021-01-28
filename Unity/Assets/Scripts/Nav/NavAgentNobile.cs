using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class NavAgentNobile : MonoBehaviour
{
    private NavAgent<NavAgentNobile> _navAgent;
    private Animator _animator;

    private List<Vector3> _targets = new List<Vector3>();
    protected enum Behaviours{Stop, Talk, Move, Path, Interact}
    private Behaviours behaviour = Behaviours.Stop;

    void Start()
    {
        _navAgent = new NavAgent<NavAgentNobile>(gameObject, new FiniteStateMachine<NavAgentNobile>(gameObject));
        _animator = gameObject.GetComponent<Animator>();
        AddStatesAndTransitions();
    }

    void Update()
    {
        List<bool> bools = new List<bool>{false, true};
        bool b = bools[Random.Range(0, bools.Count)];
        switch (behaviour)
        {
            case Behaviours.Stop: // do nothing
                break;
            case Behaviours.Talk:
                _animator.SetBool("WalkState", false);
                _animator.SetBool("TalkState", true);
                _animator.SetInteger("TalkAnimation", Random.Range(0, 2));
                break;
            case Behaviours.Move: // reaches a position and destroys itself
                _animator.SetBool("WalkState", true);
                _animator.SetBool("TalkState", false);
                if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.walkSpeed)_animator.SetBool("Run", false);
                else if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.runSpeed)_animator.SetBool("Run", true);
                _navAgent.SetDestination(_targets[Random.Range(0, _targets.Count)], b, true);
                break;
            case Behaviours.Path: // follows the path
                _animator.SetBool("WalkState", true);
                _animator.SetBool("TalkState", false);
                if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.walkSpeed)_animator.SetBool("Run", false);
                else if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.runSpeed)_animator.SetBool("Run", true);
                _navAgent.SetDestination(_targets[Random.Range(0, _targets.Count)], b, false);
                break;
            case Behaviours.Interact: // interacts with the user and goes back to the previous state
                _animator.SetBool("WalkState", false);
                _animator.SetBool("TalkState", true);
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
            case "Stop":
                behaviour = Behaviours.Stop;
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
                behaviour = Behaviours.Path;
                break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }

    public void SetTargets(List<Vector3> targets){_targets = new List<Vector3>(targets);}
}
