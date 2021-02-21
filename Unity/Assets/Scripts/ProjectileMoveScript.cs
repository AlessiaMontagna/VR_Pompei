using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveScript : MonoBehaviour
{
    public float Speed;
    public GameObject impactPrefab;
    public List<GameObject> trails;
    public SpawnLapillusScript parentScript;
    GameObject target;  

    private Rigidbody _rb;
    private bool _last;

    // Start is called before the first frame update
    void Start(){_rb = GetComponent<Rigidbody>();}

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Speed != 0 && _rb != null)
        {  
            if(_last)_rb.position += target.transform.position * (Speed * Time.deltaTime);
            else _rb.position += transform.forward * (Speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision) 
    {
        Speed = 0;   
        ContactPoint contact = collision.contacts[0];   //Quaternion.identity
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.up); //, contact.point);
        Vector3 pos = contact.point;
        parentScript.TellHitPos(pos);                       

        if(impactPrefab != null)
        {
            var impactVFX = Instantiate(impactPrefab, pos, rot) as GameObject;
            //Check if lapil hit the floor
            if(collision.gameObject.tag != "Floor")impactVFX.GetComponent<ImpactAudioScript>().StopParticles();
            Destroy(impactVFX, 5);
        }

        if(trails.Count > 0)    //faccio durare la scia di fumo dopo che il lapillo ha colpito il terreno
        {
            for(int i = 0; i<trails.Count; i++)
            {
                trails[i].transform.parent = null;
                var ps = trails[i].GetComponent<ParticleSystem>();
                if(ps!=null)
                {
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }
        }
        Destroy(gameObject);
    } 

    public void IsLast(GameObject character)
    {
        target = character;
        _last = true;
    }

    void OnTriggerEnter(Collider collider){if(collider?.tag == "NPC")collider.GetComponent<NpcInteractable>().OnExplosion(gameObject.transform.position);}
}
