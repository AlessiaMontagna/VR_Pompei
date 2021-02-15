using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcTutorial : NpcInteractable
{
    public bool pointingSchiavo = false; 
    public bool hasTalked = false;
    private int index = 0;

    // Start is called before the first frame update
    void Start(){if(navAgent == null) Initialize(Characters.Tutorial, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);}

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        hasTalked = true;
        if(character == Characters.Tutorial) StartCoroutine(TutorialTalk());
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
        if(!FindObjectOfType<NavSpawner>().navspawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("SPAWN ERROR");
        Initialize(Characters.Schiavo, FindObjectOfType<NavSpawner>().gameObject, "Move", new List<Vector3>{spawns.ElementAt(Random.Range(0, spawns.Count)).transform.position});
    }
}
