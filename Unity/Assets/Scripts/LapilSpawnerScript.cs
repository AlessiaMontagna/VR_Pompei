using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapilSpawnerScript : MonoBehaviour
{
    public GameObject prefab;
    public GameObject last;
    public GameObject character;
    public LevelChangerScript level;
    float offset = 50.0f;
    public bool running = false;
    public float min = 0.0f;
    public float max = 3.0f;    

    public IEnumerator Rain()
    {
        running = true;
        //Debug.Log("Coroutine Started.");
        Vector3 position = new Vector3(Random.Range(transform.position.x-offset, transform.position.x+offset), 0, Random.Range(transform.position.z-offset, transform.position.z+offset));
        var prefabVFX = Instantiate(prefab, position, Quaternion.identity);
        prefabVFX.GetComponent<SpawnLapillusScript>().Normal(); 
        Destroy(prefabVFX, 5);
        yield return new WaitForSeconds(Random.Range(min, max));
        running = false;
        //Debug.Log("Coroutine finished.");
    }

    public void LoadFinalScene()
    {
        if(level != null)level.FadeToNextLevel();
        else
        {
            Debug.LogError("Level error");
            return;
        }
        //SceneLoader.Load(SceneLoader.Scene.ScenaFinale);
    }

    public void kill_player()
    {
        Debug.Log("Killing player!!!");
        Vector3 position = character.transform.position;
        var prefabVFX = Instantiate(last, position, Quaternion.identity);
        prefabVFX.GetComponent<SpawnLapillusScript>().Normal();    
        Destroy(prefabVFX, 5);
        Debug.Log("Loading Scene...");
        LoadFinalScene();
    }

    public void ComputeDistance(Vector3 pos)
    {
        if(pos != null)
        {
            //Debug.Log("FOUND IT!!!2");
            float distance = Vector3.Distance(pos, character.transform.position);
            //Debug.Log("Distance: "+ distance);
            if(distance <= 5)
            {
                Debug.Log("DEAD!!!");
                LoadFinalScene();
            }
        } 
        else
        {
            //Debug.Log("NOTHING!!!2");
            return;
        }      
    }

}
