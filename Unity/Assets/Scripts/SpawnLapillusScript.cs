using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLapillusScript : MonoBehaviour
{
    public GameObject Vfx;
    public Transform StartPoint;
    public Transform EndPoint;
    GameObject objVFX;
    GameObject target; 

    // Start is called before the first frame update
    /*wvoid Start()
    {                   
        var startPos = StartPoint.position;
        GameObject objVFX = Instantiate (Vfx, startPos, Quaternion.identity) as GameObject;                
        var endPos = EndPoint.position;

        RotateTo (objVFX, endPos);         

        Destroy(gameObject, 3);       
    }*/

    public void Normal()
    {
        var startPos = StartPoint.position;
        GameObject objVFX = Instantiate (Vfx, startPos, Quaternion.identity) as GameObject;                
        var endPos = EndPoint.position;

        RotateTo (objVFX, endPos);         

        Destroy(gameObject, 3);
    }

    public void IsLast(GameObject character)
    {
        target = character;
        var startPos = StartPoint.position;
        GameObject objVFX = Instantiate (Vfx, startPos, Quaternion.identity) as GameObject;        
        var endPos = target.transform.position;
        //RotateTo (objVFX, endPos);
        objVFX.GetComponent<ProjectileMoveScript>().IsLast(target); 
        Destroy(gameObject, 10);                     
    }

    void RotateTo (GameObject obj, Vector3 destination)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation (direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1); //interpolation from A to B
    }

    public void TellHitPos(Vector3 position)
    {
        LapilSpawnerScript parentScript = GameObject.FindObjectOfType<LapilSpawnerScript>();

        if(parentScript != null)
        {
            //Debug.Log("FOUND IT!!!");
            Vector3 pos = position;
            parentScript.ComputeDistance(pos);
        }
        else
        {
            //Debug.Log("NOTHING!!!");
            return;
        }
    }
}
