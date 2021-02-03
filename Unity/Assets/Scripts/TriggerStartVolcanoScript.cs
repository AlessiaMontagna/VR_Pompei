using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStartVolcanoScript : MonoBehaviour
{
    LapilSpawnerScript lapilSpawner;
    public GameObject player;
    Collider player_collider;
    // Start is called before the first frame update
    void Start()
    {
        lapilSpawner = GameObject.FindGameObjectWithTag("TriggerStartVolcano").GetComponent<LapilSpawnerScript>();
        if(player != null)
            player_collider = player.gameObject.GetComponent<Collider>();
        else
            return;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other) 
    {
        if(other == player_collider)
        {
            Debug.Log("Entered the Trigger");
            lapilSpawner.routineStarter(0);
        }
        else
            return;
        
    }
}
