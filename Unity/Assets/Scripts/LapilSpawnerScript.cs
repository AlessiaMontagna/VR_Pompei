using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapilSpawnerScript : MonoBehaviour
{
    public GameObject prefab;
    public GameObject character;
    public LevelChangerScript level;
    float offset = 50.0f;
    public bool running = false;
    public float min = 0.0f;
    public float max = 3.0f;

public IEnumerator Rain() 
{   
    running = true;
    Debug.Log("Coroutine Started.");
    Vector3 position = new Vector3(Random.Range(transform.position.x-offset, transform.position.x+offset), 0, Random.Range(transform.position.z-offset, transform.position.z+offset));
    var prefabVFX = Instantiate(prefab, position, Quaternion.identity);
    Destroy(prefabVFX, 5);
    yield return new WaitForSeconds(Random.Range(min, max));  
    running = false;
    Debug.Log("Coroutine finished.");
}

public void LoadFinalScene()
{
    if(level != null)
    {
        level.FadeToNextLevel();
    } else
    {
        Debug.Log("Level error");
        return;
    }
    //SceneLoader.Load(SceneLoader.Scene.ScenaFinale);
}

public void kill_player(GameObject player)
{
    Debug.Log("Killing player!!!");
    Vector3 position = player.transform.position;
    var prefabVFX = Instantiate(prefab, position, Quaternion.identity);
    Destroy(prefabVFX, 5);
    Debug.Log("Loading Scene...");
    LoadFinalScene();
}

}
