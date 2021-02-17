using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuakeScript : MonoBehaviour
{
    public GameObject player;
    public CameraShakeScript cameraShake;
    Collider player_collider;
    bool first;
    // Start is called before the first frame update
    void Start()
    {
        if(player != null)
            player_collider = player.gameObject.GetComponent<Collider>();
        else
            return;
    }
        
   void OnTriggerEnter(Collider other)
   {
       if(other == player_collider && !first)
        {
            //Debug.Log("Entered the Trigger");
            Globals.earthquake = true;
            first = true;
            StartCoroutine(cameraShake.Shake(10f, .2f));     //(duration, magnitude)
        }
        else
            return;
   }
}
