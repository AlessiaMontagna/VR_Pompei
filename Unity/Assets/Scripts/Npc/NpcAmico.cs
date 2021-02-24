using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcAmico : NpcInteractable
{
    private bool _firstTalk = true;

    void Start()
    {
        if(navAgent != null)return;
        Initialize(Characters.Amico, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);
        gameObject.transform.parent = FindObjectsOfType<NavElement>().Where(i => i != null && i.subrole == NavSubroles.AmicoStop).ElementAt(0).gameObject.transform;
    }

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
