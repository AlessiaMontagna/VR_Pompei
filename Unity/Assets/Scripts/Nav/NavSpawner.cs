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
    [SerializeField] private List<GameObject> _birdPrefabs = new List<GameObject>();
    [SerializeField] private int _nGuards;
    [SerializeField] private int _nPeople;
    [SerializeField] private int _nBirds;

    private int _people = 0;
    private int _guards = 0;
    private int _birds = 0;
    private readonly int MAXPEOPLE = 15;
    private readonly int MAXGUARDS = 7;
    private readonly int MAXBIRDS = 10;

    private Dictionary<string, List<Transform>> _stops = new Dictionary<string, List<Transform>>();
    private Dictionary<string, List<Vector3>> _paths = new Dictionary<string, List<Vector3>>();
    private List<Vector3> _spawns = new List<Vector3>();

    void Start()
    {
        // check PREFABS lists
        if(_schiavoPrefabs.Count <= 0 || _mercantePrefabs.Count <= 0 || _nobilePrefabs.Count <= 0 || _guardPrefabs.Count <= 0 || _birdPrefabs.Count <= 0) Debug.LogError("AT LEAST 1 PREFAB PER TYPE MUST BE DEFINED");
        // get STOPS PATHS and SPAWNS
        foreach (var item in GameObject.FindObjectsOfType<NavElement>().Where(i => i != null))
        {
            var role = item.GetComponent<NavElement>().GetRole();
            if(role.Contains("Spawn")) _spawns.Add(new Vector3(item.transform.position.x, 0, item.transform.position.z));
            else if(role.Contains("Stop"))
            {
                if(_stops.TryGetValue(role, out var list)) list.Add(item.transform);
                else _stops.Add(role, new List<Transform>{item.transform});
            }
            else if(role.Contains("Path"))
            {
                if(_paths.TryGetValue(role, out var list)) list.Add(new Vector3(item.transform.position.x, 0, item.transform.position.z));
                else _paths.Add(role, new List<Vector3>{new Vector3(item.transform.position.x, 0, item.transform.position.z)});
            }
        }
        // spawn STOPS guards
        if(!_stops.TryGetValue("GuardStop", out var stops)) Debug.LogError("GUARD STOPS MISSING IN SPAWN LIST");
        else foreach (var item in stops.Where(i => i != null)){SpawnAgent(false, _guardPrefabs.ElementAt(Random.Range(0, _guardPrefabs.Count)), "NavAgentGuard", "Idle", item.position, item.rotation, null);}
        // spawn STOPS mercanti
        if(!_stops.TryGetValue("MercanteStop", out stops)) Debug.LogError("MERCANTI STOPS MISSING IN SPAWN LIST");
        else foreach (var item in stops.Where(i => i != null)){SpawnAgent(false, _mercantePrefabs.ElementAt(Random.Range(0, _mercantePrefabs.Count)), "NavAgentMercante", "Idle", item.position, item.rotation, null);}
        // spawn STOPS balcony
        if(!_stops.TryGetValue("BalconyStop", out stops)) Debug.LogError("BALCONY STOPS MISSING IN SPAWN LIST");
        else foreach (var item in stops.Where(i => i != null))
        {
            if(Random.Range(0f, 1f) < 0.3f){SpawnAgent(false, _nobilePrefabs.ElementAt(Random.Range(0, _nobilePrefabs.Count)), "NavAgentNobile", "Idle", item.position, item.rotation, null);}
            else{SpawnAgent(false, _mercantePrefabs.ElementAt(Random.Range(0, _mercantePrefabs.Count)), "NavAgentMercante", "Idle", item.position, item.rotation, null);}
        }
        // STOPS groups
        if(!_stops.TryGetValue("GroupStop", out stops)) Debug.LogError("GROUP STOPS MISSING IN SPAWN LIST");
        else foreach (var item in stops.Where(i => i != null))
        {
            GameObject agent;
            Vector3 position;
            Quaternion rotation;
            int count = Random.Range(2, 5);
            for (int i = 0; i < count; i++)
            {
                do{var random = Random.insideUnitCircle.normalized * Random.Range(1f, 1.1f);position = item.position + new Vector3(random.x, 0, random.y);}
                while(!UnityEngine.AI.NavMesh.SamplePosition(position, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas));
                rotation = Quaternion.LookRotation(item.position-position, Vector3.up);
                if(Random.Range(0f, 1f) < 0.3f) agent = SpawnAgent(false, _mercantePrefabs.ElementAt(Random.Range(0, _mercantePrefabs.Count)), "NavAgentMercante", "Talk", position,  rotation, null);
                else agent = SpawnAgent(false, _nobilePrefabs.ElementAt(Random.Range(0, _nobilePrefabs.Count)), "NavAgentNobile", "Talk", position, rotation, null);
            }
        }
        // SET agents total numbers
        if(_nPeople < 0 || _nPeople > MAXPEOPLE) _nPeople = Random.Range(7, MAXPEOPLE+1);
        if(_nGuards < 0 || _nGuards > MAXGUARDS) _nGuards = Random.Range(2, MAXGUARDS+1);
        if(_nBirds < 0 || _nBirds > MAXBIRDS) _nBirds = Random.Range(1, MAXBIRDS+1);
    }

    void Update()
    {
        // SPAWN agents if there are less then defined in Start()
        while(_nGuards > _guards){var path = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;SpawnAgent(true, _guardPrefabs.ElementAt(Random.Range(0, _guardPrefabs.Count)), "NavAgentGuard", "Path", path.ElementAt(Random.Range(0, path.Count)), Quaternion.identity, path);}
        while(_nPeople > _people)
        {
            var dest = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;
            dest.Add(_spawns.ElementAt(Random.Range(0, _spawns.Count)));
            if(Random.Range(0f, 1f) < 0.2f) SpawnAgent(true, _schiavoPrefabs.ElementAt(Random.Range(0, _schiavoPrefabs.Count)), "NavAgentSchiavo", "Move", _spawns.ElementAt(Random.Range(0, _spawns.Count)), Quaternion.identity, dest);
            else if(Random.Range(0f, 1f) < 0.4f) SpawnAgent(true, _mercantePrefabs.ElementAt(Random.Range(0, _mercantePrefabs.Count)), "NavAgentMercante", "Move", _spawns.ElementAt(Random.Range(0, _spawns.Count)), Quaternion.identity, dest);
            else SpawnAgent(true, _nobilePrefabs.ElementAt(Random.Range(0, _nobilePrefabs.Count)), "NavAgentNobile", "Move", _spawns.ElementAt(Random.Range(0, _spawns.Count)), Quaternion.identity, dest);
        }
        while(_nBirds > _birds){SpawnAgent(true, _birdPrefabs.ElementAt(Random.Range(0, _birdPrefabs.Count)), "NavAgentBird", "Path", _spawns.ElementAt(Random.Range(0, _spawns.Count)), Quaternion.identity, _paths.ElementAt(Random.Range(0, _paths.Count)).Value);}
    }

    private GameObject SpawnAgent(bool count, GameObject prefab, string role, string state, Vector3 position, Quaternion rotation, List<Vector3> targets)
    {
        GameObject agent = Instantiate(prefab, position, rotation);
        switch (role)
        {
            case "NavAgentSchiavo":
                agent.AddComponent<NavAgentSchiavo>();
                agent.GetComponent<NavAgentSchiavo>().SetState(state);
                if(targets != null)agent.GetComponent<NavAgentSchiavo>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentMercante":
                agent.AddComponent<NavAgentMercante>();
                agent.GetComponent<NavAgentMercante>().SetState(state);
                if(targets != null)agent.GetComponent<NavAgentMercante>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentNobile":
                agent.AddComponent<NavAgentNobile>();
                agent.GetComponent<NavAgentNobile>().SetState(state);
                if(targets != null)agent.GetComponent<NavAgentNobile>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentGuard":
                agent.AddComponent<NavAgentGuard>();
                agent.GetComponent<NavAgentGuard>().SetState(state);
                if(targets != null)agent.GetComponent<NavAgentGuard>().SetTargets(targets);
                if(count)_guards++;
                break;
            case "NavAgentBird":
                agent.AddComponent<NavAgentBird>();
                agent.GetComponent<NavAgentBird>().SetState(state);
                if(targets != null)agent.GetComponent<NavAgentBird>().SetTargets(targets);
                if(count)_birds++;
                break;
            default: throw new System.ArgumentOutOfRangeException();
        }
        //if(lookat != null)agent.transform.LookAt(lookat.transform.position);
        // set agent as child of the spawner
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
            case "NavAgentBird": _birds--;break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }
}