using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTutorial : NpcInteractable
{
    public bool pointingSchiavo = false; 
    public bool hasTalked = false;

    // Start is called before the first frame update
    void Start(){if(navAgent == null) Initialize(Characters.SchiavoTutorial, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);}

    protected override void StartInteraction()
    {
        if(charcter == Characters.SchiavoTutorial)StartCoroutine(DialogTalk());
    }

    private IEnumerator DialogTalk()
    {
        SetSubtitles(audioSubManager.GetSubs(index));
        Globals.someoneIsTalking = true;
        yield return new WaitForSeconds(audio.length);
        Globals.someoneIsTalking = false;
        _sottotitoli.text ="";
        yield return new WaitForSeconds(2f);
        //TODO: schiavo se ne deve andare via. 
    }
}
