﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class NavAgentGuard : MonoBehaviour
{
    private NavAgent<NavAgentGuard> _navAgent;
    private Animator _animator;

    private List<Vector3> _targets = new List<Vector3>();

    void Start()
    {
        _navAgent = new NavAgent<NavAgentGuard>(gameObject, new FiniteStateMachine<NavAgentGuard>(gameObject));
        _animator = gameObject.GetComponent<Animator>();
        OverrideStatesAndTransitions();
    }

    void Update()
    {
        //if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.walkSpeed)_animator.SetBool("Run", false);
        //else if(gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed == _navAgent.runSpeed)_animator.SetBool("Run", true);
        _navAgent.Tik();
    }

    private void OverrideStatesAndTransitions()
    {
        State interact = _navAgent.AddState("Interact", () => {}, () => {}, () => {});
        _navAgent.AddTransition(_navAgent.GetState("Talk"), interact, () => Random.Range(0f, 1f) < 0.1f);
        _navAgent.AddTransition(interact, _navAgent.GetState("Talk"), () => Random.Range(0f, 1f) < 0.6f);
    }

    public void SetState(string statename){_navAgent.SetState(statename);}

    public void SetTargets(List<Vector3> targets){_targets = new List<Vector3>(targets);}
}
