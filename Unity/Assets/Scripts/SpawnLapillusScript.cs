using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLapillusScript : MonoBehaviour
{
    public GameObject Vfx;
    public Transform StartPoint;
    public Transform EndPoint;    
    
    // Start is called before the first frame update
    void Start()
    {        
        var startPos = StartPoint.position;
        GameObject objVFX = Instantiate (Vfx, startPos, Quaternion.identity) as GameObject;        

        var endPos = EndPoint.position;

        RotateTo (objVFX, endPos); 

        Destroy(gameObject, 3);       
    }

    void RotateTo (GameObject obj, Vector3 destination)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation (direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1); //interpolation from A to B
    }
}
