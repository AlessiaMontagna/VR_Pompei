using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuakeScript : MonoBehaviour
{
    public GameObject player;
    public CameraShakeScript cameraShake;
    Collider player_collider;
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
       if(other == player_collider)
        {
            Debug.Log("Entered the Trigger");
            StartCoroutine(cameraShake.Shake(5f, .2f));
        }
        else
            return;
   }
}
