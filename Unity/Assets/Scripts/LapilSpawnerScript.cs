using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapilSpawnerScript : MonoBehaviour
{
    public GameObject prefab;
    //public GameObject character;
    float offset = 50.0f; 

IEnumerator Rain() 
{
    yield return new WaitForSeconds(Random.Range(0,3));
    Vector3 position = new Vector3(Random.Range(transform.position.x-offset, transform.position.x+offset), 0, Random.Range(transform.position.z-offset, transform.position.z+offset));
    Instantiate(prefab, position, Quaternion.identity);
    var timer = 5f;
    while (timer>0f)
    {
        timer -= 1 * Time.deltaTime;                          
    }
    Destroy(prefab);
    routineStarter(0);
}

public void routineStarter(int code)
{
    if(code == 1)
    {
        Update();
    }        
    else
    {
        StartCoroutine("Rain");  
    }
        
}

public void kill_player(GameObject player)
{
    Debug.Log("Killing player!!!");
    Vector3 position = player.transform.position;
    Instantiate(prefab, position, Quaternion.identity);
    routineStarter(1);
}

void Update() 
{
    
}

}
