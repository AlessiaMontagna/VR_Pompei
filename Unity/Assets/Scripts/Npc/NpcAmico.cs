using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAmico : NpcInteractable
{
    private bool _firstTalk = true;

    void Start(){if(navAgent == null) Initialize(Characters.Amico, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);}

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        int minIndex = 0;
        if(Globals.player == Players.Nobile)
        {
            if(_firstTalk)
            {
                FindObjectOfType<NpcMySchiavo>().swoosh = true;
                FindObjectOfType<MissionManager>().UpdateMission(Missions.Mission2_FindSlave);
                _firstTalk = false;
                StartCoroutine(Talk(0));
                return;
            }
            else minIndex = 1;
        }
        else minIndex = 0;
        StartCoroutine(Talk(Random.Range(minIndex, audioSubManager.GetMaxAudios(Characters.Amico, voice))));
    }
}
