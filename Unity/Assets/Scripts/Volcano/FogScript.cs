using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    public GameObject player;
    float _delta = 0.002f;

    void OnTriggerExit(Collider other) 
    {        
        if(other.gameObject == player)
        {
            Debug.Log("Player entered the Trigger: Fog");
            StopCoroutine("Fog");
            float a = RenderSettings.fogDensity;
            Debug.Log("FogDensity: " + a);
            StartCoroutine(Fog(a, _delta));                      
        }
        else
            return;        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            Debug.Log("Player exited the Trigger: Fog");
            StopCoroutine("Fog"); 
            float b = -_delta;
            float a = RenderSettings.fogDensity;
            Debug.Log("FogDensity: " + a);       
            StartCoroutine(Fog(a, b));      
        }
        else
            return; 
    }

    private IEnumerator Fog(float density, float delta)
    {        
        RenderSettings.fogDensity = density;
        if(!RenderSettings.fog) RenderSettings.fog = true;                
        if(delta > 0)
        {
            Debug.Log("Fogging In...");
            for(float f = density; f <= 0.05; f+= delta)
            {
                RenderSettings.fogDensity = f;
                Debug.Log("f: " + f);
                yield return new WaitForSeconds(delta);
            }
        } else 
        {
            Debug.Log("Fogging Out...");
            for(float f = density; f >= 0.0f; f+= delta)
            {
                RenderSettings.fogDensity = f;
                Debug.Log("-f: " + f);
                yield return new WaitForSeconds(-delta);
            }
            RenderSettings.fogDensity = 0.0f;
        }
        
    } 
}
