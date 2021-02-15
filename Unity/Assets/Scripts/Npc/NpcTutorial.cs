using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcTutorial : NpcInteractable
{
    public bool pointingSchiavo = false; 
    public bool hasTalked = false;

    // Start is called before the first frame update
    void Start(){if(navAgent == null) Initialize(Characters.SchiavoTutorial, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);}

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        pointingSchiavo = true;
        if(character == Characters.SchiavoTutorial) StartCoroutine(TutorialTalk());
        else base.StartInteraction();
    }

    private IEnumerator TutorialTalk()
    {
        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        SetAudio(0);
        SetSubtitles(0);
        Globals.someoneIsTalking = true;
        yield return new WaitForSeconds(GetAudioLength()+2f);
        hasTalked = true;
        StopInteraction();
        if(!FindObjectOfType<NavSpawner>().navspawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("SPAWN ERROR");
        Initialize(Characters.Schiavo, FindObjectOfType<NavSpawner>().gameObject, "Move", new List<Vector3>{spawns.ElementAt(Random.Range(0, spawns.Count)).transform.position});
    }
}
