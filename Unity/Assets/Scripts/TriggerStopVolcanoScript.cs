using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStopVolcanoScript : MonoBehaviour
{
    public LapilSpawnerScript lapils;
    public GameObject player;    
    Collider player_collider;
    private bool stop = false;

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject == player && stop == false)
        {
            Debug.Log("Player entered the Trigger: STOP Lapills!!!");
            stop = true;
            Debug.Log("Stopping lappils...");
            lapils.kill_player(player);
        }
        else
            return;
        
    }

    public bool getState()
    {
        return stop;
    }
}
