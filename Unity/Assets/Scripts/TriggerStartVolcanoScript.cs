using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStartVolcanoScript : MonoBehaviour
{
    public LapilSpawnerScript lapils;    
    public TriggerStopVolcanoScript trig_stop;
    public GameObject player;
    Collider pcol;
    public bool first_time = false;
    
    void OnTriggerEnter(Collider other) 
    {        
        if(other.gameObject == player && !first_time)
        {
            Debug.Log("Player entered the Trigger: START Lapills");
            first_time = true;                      
        }
        else
            return;        
    }

    private void Update() 
    {
        if(trig_stop.getState() == false && first_time == true && lapils.running == false) //finchè non tocco il trigger di fine, continuo coi lapilli
        {            
            StartCoroutine(lapils.Rain());            
        }
    }    
}
