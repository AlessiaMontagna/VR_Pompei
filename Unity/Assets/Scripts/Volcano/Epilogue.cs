using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epilogue : MonoBehaviour
{
    public GameObject Player;
    public GameObject SmokeColumn;
    public GameObject Barrier;
    public GameObject Barriers;
    public GameObject Debris;
    public GameObject Fire;
    public LevelChangerScript level;
    public Transform ExplosionSpot;
    public CameraShakeScript cameraShake;
    public GameObject VolcanoExplosion; 
    public float radius;
    public float power;
    bool _first = false;    

    void Awake() 
    {
        SmokeColumn.SetActive(false);
        Barriers.SetActive(false);
        AudioListener.pause = false;
        VolcanoExplosion.SetActive(false);                 
    }
   
    void OnTriggerEnter(Collider other) 
    {        
        if(other.gameObject == Player && !_first)
        {
            Debug.Log("Player entered the Trigger: Epiogue");
            _first = true;            
            Barrier.SetActive(true);
            Vector3 explosionPos = ExplosionSpot.position;
            Explode(explosionPos, radius, power, Debris, Fire, cameraShake);              
            StartCoroutine("Smoke");
        }
        else
            return;        
    }

    public IEnumerator Smoke()
    {
        yield return new WaitForSeconds(1.0f);
        Barriers.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        VolcanoExplosion.SetActive(true);
        yield return new WaitForSeconds(10.0f);  
        SmokeColumn.SetActive(true);
        yield return new WaitForSeconds(11.3f);
        level.FadeToLevel(0); //torna al menu  
        AudioListener.pause = true;      
    }

    public void Explode(Vector3 explosionPos, float radius, float power, GameObject Debris, GameObject Fire, CameraShakeScript cameraShake)
    {
        //Vector3 explosionPos = ExplosionSpot.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        var explosionVFX = Instantiate(Debris, explosionPos, Quaternion.identity);                
        var fireVFX = Instantiate(Fire, explosionPos, Quaternion.identity);
        StartCoroutine(cameraShake.Shake(0.5f, 0.3f));

        foreach (Collider hit in colliders)
        {
            if(hit.gameObject != Player)
            {            
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                
                if(rb != null)
                {
                    if(rb.isKinematic)
                        rb.isKinematic = false;
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0f);
                }
            }                                        
        }
    }
}
