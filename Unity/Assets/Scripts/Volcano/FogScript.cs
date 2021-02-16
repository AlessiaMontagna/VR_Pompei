using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    public GameObject player;
    bool _toggle = false;
    void OnTriggerEnter(Collider other) 
    {        
        if(other.gameObject == player)
        {
            Debug.Log("Player entered the Trigger: Fog");        
            RenderSettings.fogDensity = 0f;            
        }
        else
            return;        
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            Debug.Log("Player exited the Trigger: Fog");        
            RenderSettings.fogDensity = 0.05f;            
        }
        else
            return; 
    }
}
