using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : StateMachineBehaviour
{
    [SerializeField] private int idleAnimations;
    [SerializeField] private int talkAnimations;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        string nextState = "";
        int nextStateIndex = -1;
        if(animator.GetBool("Move") && animator.GetBool("Talk") && animator.GetBool("Turn") && animator.GetBool("Earthquake")){nextState = "Idle";nextStateIndex = Random.Range(0, idleAnimations);}
        if(animator.GetBool("Talk")){nextState = "Talk";nextStateIndex = Random.Range(0, talkAnimations);}
        //Debug.Log($"State: {nextState}; Animation: {nextStateIndex}");
        if(nextState != "" && nextStateIndex >= 0)animator.SetFloat($"{nextState}Float", nextStateIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
