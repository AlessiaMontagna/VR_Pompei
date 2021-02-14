using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAmico : NpcInteractable
{
    private int index = 0;

    protected override void StartInteraction()
    {
        if(Globals.player == Players.Schiavo){index = Random.Range(0, audioSubManager.GetMaxAudios(Characters.Amico, voice));}
        base.StartInteraction();
        if(Globals.player == Players.Nobile && index == 0)
        {
            FindObjectOfType<MySchiavoInteractable>()._switch = true;
            FindObjectOfType<MissionManager>().UpdateMission(Missions.Mission2_FindSlave);
            index = 1;
        }
    }
}
