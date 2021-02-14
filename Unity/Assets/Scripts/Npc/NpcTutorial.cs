using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTutorial : NpcInteractable
{
    public bool pointingSchiavo = false; 
    public int index = 0;

    // Start is called before the first frame update
    void Start(){if(navAgent == null) Initialize(Characters.SchiavoTutorial, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);}

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        if(character == Characters.SchiavoTutorial)StartCoroutine(TutorialTalk());
        else base.StartInteraction();
    }

    private IEnumerator TutorialTalk()
    {
        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        SetAudio(index);
        SetSubtitles(index);
        Globals.someoneIsTalking = true;
        yield return new WaitForSeconds(GetAudioLength()+2f);
        StopInteraction();
        Initialize(Characters.Schiavo, FindObjectOfType<NavSpawner>().gameObject, "Move", null);
    }
}
