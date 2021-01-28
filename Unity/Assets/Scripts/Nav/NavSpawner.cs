using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _peoplePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _guardPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _dogPrefabs = new List<GameObject>();
    [SerializeField] private int _nGuards;
    [SerializeField] private int _nPeople;
    [SerializeField] private int _nDogs;

    private int _people = 0;
    private int _guards = 0;
    private int _dogs = 0;
    private readonly int _peopleMax = 15;
    private readonly int _guardsMax = 4;
    private readonly int _dogsMax = 2;

    private List<Vector3> _spawns = new List<Vector3>();
    private Dictionary<string, List<Vector3>> _targets = new Dictionary<string, List<Vector3>>();

    void Start()
    {
        // PREFAB check
        if(_peoplePrefabs.Count <= 0 || _guardPrefabs.Count <= 0 || _dogPrefabs.Count <= 0) Debug.LogError("AT LEAST 1 PREFAB PER TYPE MUST BE DEFINED");
        // SPAWNS
        foreach (var item in GameObject.FindObjectsOfType<NavSpawn>().Where(i => i != null && i.tag == "Spawn")){_spawns.Add(item.transform.position);}
        // STOPS for guards to stand
        foreach (var item in GameObject.FindObjectsOfType<NavSpawn>().Where(i => i != null && i.tag == "GuardStop"))
            SpawnAgent(false, _guardPrefabs[Random.Range(0, _guardPrefabs.Count)], "NavAgentGuard", "Stop", item.transform.position, item.transform.rotation, null, null);
        // STOPS for a group of people talking each other
        foreach (var item in GameObject.FindObjectsOfType<NavSpawn>().Where(i => i != null && i.tag == "PeopleStop"))
        {
            // create group
            int count = Random.Range(2, 5);
            float sum = 360f;
            for (int i = 0; i < count; i++)
            {
                Vector3 position; // position = center + rotation angle * forward direction * radius
                do{position = item.transform.position + Quaternion.AngleAxis(i*Random.Range(25f, sum/(float)count), Vector3.up) * Vector3.forward * Random.Range(1f, 1.1f);}
                while(!UnityEngine.AI.NavMesh.SamplePosition(position, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas));
                if(Random.Range(0f, 1f) < 0.6f) SpawnAgent(false, _peoplePrefabs[Random.Range(0, _peoplePrefabs.Count)], "NavAgentPeople", "Talk", position, Quaternion.identity, null, item.gameObject);
                else SpawnAgent(false, _guardPrefabs[Random.Range(0, _guardPrefabs.Count)], "NavAgentGuard", "Talk", position, Quaternion.identity, null, item.gameObject);
            }
        }
        // TARGETS
        foreach (var item in GameObject.FindObjectsOfType<NavTarget>().Where(i => i != null))
        {
            if(_targets.TryGetValue(item.tag, out var path)) path.Add(item.transform.position);
            else _targets.Add(item.tag, new List<Vector3>{item.transform.position});
        }
        // SET agents total numbers
        if(_nPeople < 0 || _nPeople > _peopleMax) _nPeople = Random.Range(7, _peopleMax+1);
        if(_nGuards < 0 || _nGuards > _guardsMax) _nGuards = Random.Range(2, _guardsMax+1);
        if(_nDogs < 0 || _nDogs > _dogsMax) _nDogs = Random.Range(1, _dogsMax+1);
    }

    void Update()
    {
        // SPAWN agents if there are less then defined in Start()
        //Debug.Log($"There are: {_people} people, {_guards} guards, {_dogs} dogs");
        while(_nGuards > _guards){SpawnAgent(true, _guardPrefabs[Random.Range(0, _guardPrefabs.Count)], "NavAgentGuard", "Path", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, _targets.ElementAt(Random.Range(0, _targets.Count)).Value, null);}
        while(_nPeople > _people){_targets.TryGetValue("Target", out var targets);SpawnAgent(true, _peoplePrefabs[Random.Range(0, _peoplePrefabs.Count)], "NavAgentPeople", "Move", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, targets, null);}
        while(_nDogs > _dogs){_targets.TryGetValue("Target", out var targets);SpawnAgent(true, _dogPrefabs[Random.Range(0, _dogPrefabs.Count)], "NavAgentDog", "Move", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, targets, null);}
    }

    private GameObject SpawnAgent(bool count, GameObject prefab, string classname, string behaviour, Vector3 position, Quaternion rotation, List<Vector3> targets, GameObject lookat)
    {
        GameObject agent = Instantiate(prefab, position, rotation);
        switch (classname)
        {
            case "NavAgentPeople":
                agent.AddComponent<NavAgentPeople>();
                agent.GetComponent<NavAgentPeople>().SetBehaviour(behaviour);
                if(targets != null)agent.GetComponent<NavAgentPeople>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentGuard":
                agent.AddComponent<NavAgentGuard>();
                agent.GetComponent<NavAgentGuard>().SetBehaviour(behaviour);
                if(targets != null)agent.GetComponent<NavAgentGuard>().SetTargets(targets);
                if(count)_guards++;
                break;
            case "NavAgentDog":
                agent.AddComponent<NavAgentDog>();
                agent.GetComponent<NavAgentDog>().SetBehaviour(behaviour);
                if(targets != null)agent.GetComponent<NavAgentDog>().SetTargets(targets);
                if(count)_dogs++;
                break;
            default: throw new System.ArgumentOutOfRangeException();
        }
        // set agent as child of the spawner
        if(lookat != null)agent.transform.LookAt(lookat.transform.position);
        agent.transform.parent = gameObject.transform;
        return agent;
    }

    public void DestroyedAgent(string classType)
    {
        switch (classType)
        {
            case "NavAgentPeople": _people--;break;
            case "NavAgentGuard": _guards--;break;
            case "NavAgentDog": _dogs--;break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }
}