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

    void Start()
    {
        //start coroutine to play volcano eruption sound
        //start coroutine to create fog
        StartCoroutine("Fog");
    }
    
    void OnTriggerEnter(Collider other) 
    {        
        if(other.gameObject == player && !first_time)
        {
            //Debug.Log("Player entered the Trigger: START Lapills");
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

    private IEnumerator Fog()
    {
        RenderSettings.fogDensity = 0;
        RenderSettings.fog = true;        
        yield return new WaitForSeconds(10.0f);
        for(float f = 0.0001f; f <= 0.05; f+= 0.0001f)
        {
            RenderSettings.fogDensity = f;
            yield return new WaitForSeconds(0.0001f);
        }
    }  
}
