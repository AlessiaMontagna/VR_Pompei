using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _schiavoPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _mercantePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _nobilePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _guardPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _dogPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _birdPrefabs = new List<GameObject>();
    [SerializeField] private int _nGuards;
    [SerializeField] private int _nPeople;
    [SerializeField] private int _nDogs = 0;
    [SerializeField] private int _nBirds;

    private int _people = 0;
    private int _guards = 0;
    private int _dogs = 0;
    private int _birds = 0;
    private readonly int _peopleMax = 15;
    private readonly int _guardsMax = 5;
    private readonly int _dogsMax = 2;
    private readonly int _birdsMax = 10;

    private List<Vector3> _spawns = new List<Vector3>();
    private Dictionary<string, List<Vector3>> _targets = new Dictionary<string, List<Vector3>>();

    void Start()
    {
        // PREFABS check
        if(_schiavoPrefabs.Count <= 0 || _mercantePrefabs.Count <= 0 || _nobilePrefabs.Count <= 0 || _guardPrefabs.Count <= 0 || /*_dogPrefabs.Count <= 0 || */_birdPrefabs.Count <= 0) Debug.LogError("AT LEAST 1 PREFAB PER TYPE MUST BE DEFINED");
        // get SPAWNS
        foreach (var item in GameObject.FindObjectsOfType<NavSpawn>().Where(i => i != null && i.GetComponent<NavSpawn>().GetType() == "Spawn")){_spawns.Add(item.transform.position);}
        // spawn guards in STOPS
        foreach (var item in GameObject.FindObjectsOfType<NavSpawn>().Where(i => i != null && i.GetComponent<NavSpawn>().GetType() == "GuardStop"))
            SpawnAgent(false, _guardPrefabs[Random.Range(0, _guardPrefabs.Count)], "NavAgentGuard", "Stop", item.transform.position, item.transform.rotation, null, null);
        // spawn mercante in STOPS
        foreach (var item in GameObject.FindObjectsOfType<NavSpawn>().Where(i => i != null && i.GetComponent<NavSpawn>().GetType() == "MercanteStop"))
            SpawnAgent(false, _mercantePrefabs[Random.Range(0, _mercantePrefabs.Count)], "NavAgentMercante", "Stop", item.transform.position, item.transform.rotation, null, null);
        // spawn groups in STOPS
        foreach (var item in GameObject.FindObjectsOfType<NavSpawn>().Where(i => i != null && i.GetComponent<NavSpawn>().GetType() == "PeopleGroup"))
        {
            // create group
            int count = Random.Range(2, 5);
            float sum = 360f;
            for (int i = 0; i < count; i++)
            {
                Vector3 position; // position = center + rotation angle * forward direction * radius
                do{position = item.transform.position + Quaternion.AngleAxis(i*Random.Range(25f, sum/(float)count), Vector3.up) * Vector3.forward * Random.Range(1f, 1.1f);}
                while(!UnityEngine.AI.NavMesh.SamplePosition(position, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas));
                if(Random.Range(0f, 1f) < 0.3f) SpawnAgent(false, _mercantePrefabs[Random.Range(0, _mercantePrefabs.Count)], "NavAgentMercante", "Talk", position, Quaternion.identity, null, item.gameObject);
                else SpawnAgent(false, _nobilePrefabs[Random.Range(0, _nobilePrefabs.Count)], "NavAgentNobile", "Talk", position, Quaternion.identity, null, item.gameObject);
            }
        }
        // TARGETS
        foreach (var item in GameObject.FindObjectsOfType<NavTarget>().Where(i => i != null))
        {
            if(_targets.TryGetValue(item.GetComponent<NavSpawn>().GetType(), out var path)) path.Add(item.transform.position);
            else _targets.Add(item.GetComponent<NavSpawn>().GetType(), new List<Vector3>{item.transform.position});
        }
        // SET agents total numbers
        if(_nPeople < 0 || _nPeople > _peopleMax) _nPeople = Random.Range(7, _peopleMax+1);
        if(_nGuards < 0 || _nGuards > _guardsMax) _nGuards = Random.Range(2, _guardsMax+1);
        //if(_nDogs < 0 || _nDogs > _dogsMax) _nDogs = Random.Range(1, _dogsMax+1);
        if(_nBirds < 0 || _nBirds > _dogsMax) _nBirds = Random.Range(1, _birdsMax+1);
    }

    void Update()
    {
        // SPAWN agents if there are less then defined in Start()
        while(_nGuards > _guards){SpawnAgent(true, _guardPrefabs[Random.Range(0, _guardPrefabs.Count)], "NavAgentGuard", "Path", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, _targets.ElementAt(Random.Range(0, _targets.Count)).Value, null);}
        while(_nPeople > _people)
        {
            // to spawn people must choose from schiavi mercanti e 
            _targets.TryGetValue("Target", out var targets);
            float random = Random.Range(0f, 1f);
            if(random < 0.2f) SpawnAgent(false, _schiavoPrefabs[Random.Range(0, _schiavoPrefabs.Count)], "NavAgentSchiavo", "Move", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, targets, null);
            else if(random < 0.4f) SpawnAgent(false, _mercantePrefabs[Random.Range(0, _mercantePrefabs.Count)], "NavAgentMercante", "Move", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, targets, null);
            else SpawnAgent(false, _nobilePrefabs[Random.Range(0, _nobilePrefabs.Count)], "NavAgentNobile", "Move", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, targets, null);
        }
        while(_nDogs > _dogs){_targets.TryGetValue("Target", out var targets);SpawnAgent(true, _dogPrefabs[Random.Range(0, _dogPrefabs.Count)], "NavAgentDog", "Move", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, targets, null);}
        while(_nBirds > _birds){_targets.TryGetValue("Target", out var targets);SpawnAgent(true, _birdPrefabs[Random.Range(0, _birdPrefabs.Count)], "NavAgentBird", "Path", _spawns[Random.Range(0,_spawns.Count)], Quaternion.identity, targets, null);}
    }

    private GameObject SpawnAgent(bool count, GameObject prefab, string classname, string behaviour, Vector3 position, Quaternion rotation, List<Vector3> targets, GameObject lookat)
    {
        GameObject agent = Instantiate(prefab, position, rotation);
        switch (classname)
        {
            case "NavAgentSchiavo":
                agent.AddComponent<NavAgentSchiavo>();
                agent.GetComponent<NavAgentSchiavo>().SetBehaviour(behaviour);
                if(targets != null)agent.GetComponent<NavAgentSchiavo>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentMercante":
                agent.AddComponent<NavAgentMercante>();
                agent.GetComponent<NavAgentMercante>().SetBehaviour(behaviour);
                if(targets != null)agent.GetComponent<NavAgentMercante>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentNobile":
                agent.AddComponent<NavAgentNobile>();
                agent.GetComponent<NavAgentNobile>().SetBehaviour(behaviour);
                if(targets != null)agent.GetComponent<NavAgentNobile>().SetTargets(targets);
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
            case "NavAgentBird":
                agent.AddComponent<NavAgentBird>();
                agent.GetComponent<NavAgentBird>().SetBehaviour(behaviour);
                if(targets != null)agent.GetComponent<NavAgentBird>().SetTargets(targets);
                if(count)_birds++;
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
            case "NavAgentSchiavo": _people--;break;
            case "NavAgentMercante": _people--;break;
            case "NavAgentNobile": _people--;break;
            case "NavAgentGuard": _guards--;break;
            case "NavAgentDog": _dogs--;break;
            case "NavAgentBird": _birds--;break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }
}