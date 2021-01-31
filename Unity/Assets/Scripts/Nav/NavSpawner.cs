using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _schiavoPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _mercantePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _patrizioPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _patriziaPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _guardPrefabs = new List<GameObject>();
    [SerializeField] private int _nGuards;
    [SerializeField] private int _nPeople;

    private List<string> _classes = new List<string>{"NavAgentSchiavo", "NavAgentMercante", "NavAgentPatrizio", "NavAgentPatrizia", "NavAgentGuard"};

    private int _people = 0;
    private int _guards = 0;
    private readonly int MAXPEOPLE = 15;
    private readonly int MAXGUARDS = 7;

    private Dictionary<string, List<Transform>> _stops = new Dictionary<string, List<Transform>>();
    private Dictionary<string, List<Vector3>> _paths = new Dictionary<string, List<Vector3>>();
    private List<Vector3> _spawns = new List<Vector3>();

    void Start()
    {
        // check PREFABS lists
        if(_schiavoPrefabs.Count <= 0 || _mercantePrefabs.Count <= 0 || _patrizioPrefabs.Count <= 0 || _patriziaPrefabs.Count <= 0 || _guardPrefabs.Count <= 0) Debug.LogError("AT LEAST 1 PREFAB PER TYPE MUST BE DEFINED");
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
            // TODO: POSE
            GameObject prefab;
            string classname;
            if(Random.Range(0f, 1f) < 0.3f)
            {
                if(Random.Range(0f, 1f) < 0.5f){prefab = _patrizioPrefabs.ElementAt(Random.Range(0, _patrizioPrefabs.Count));classname = "NavAgentPatrizio";}
                else {prefab = _patriziaPrefabs.ElementAt(Random.Range(0, _patriziaPrefabs.Count));classname = "NavAgentPatrizia";}
            }
            else{prefab = _mercantePrefabs.ElementAt(Random.Range(0, _mercantePrefabs.Count));classname = "NavAgentMercante";}
            SpawnAgent(false, prefab, classname, "Idle", item.position, item.rotation, null);
        }
        // STOPS groups
        if(!_stops.TryGetValue("GroupStop", out stops)) Debug.LogError("GROUP STOPS MISSING IN SPAWN LIST");
        else foreach (var item in stops.Where(i => i != null))
        {
            Vector3 position;
            Quaternion rotation;
            int count = Random.Range(2, 5);
            for (int i = 0; i < count; i++)
            {
                do{var random = Random.insideUnitCircle.normalized * Random.Range(1f, 1.1f);position = item.position + new Vector3(random.x, 0, random.y);}
                while(!UnityEngine.AI.NavMesh.SamplePosition(position, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas));
                rotation = Quaternion.LookRotation(item.position-position, Vector3.up);
                GameObject prefab;
                string classname;
                if(Random.Range(0f, 1f) < 0.3f)
                {
                    if(Random.Range(0f, 1f) < 0.5f){prefab = _patrizioPrefabs.ElementAt(Random.Range(0, _patrizioPrefabs.Count));classname = "NavAgentPatrizio";}
                    else {prefab = _patriziaPrefabs.ElementAt(Random.Range(0, _patriziaPrefabs.Count));classname = "NavAgentPatrizia";}
                }
                else{prefab = _mercantePrefabs.ElementAt(Random.Range(0, _mercantePrefabs.Count));classname = "NavAgentMercante";}
                SpawnAgent(false, prefab, classname, "Talk", position, rotation, null);
            }
        }
        // SET agents total numbers
        if(_nPeople < 0 || _nPeople > MAXPEOPLE) _nPeople = Random.Range(7, MAXPEOPLE+1);
        if(_nGuards < 0 || _nGuards > MAXGUARDS) _nGuards = Random.Range(2, MAXGUARDS+1);
    }

    void Update()
    {
        // SPAWN agents if there are less then defined in Start()
        while(_nGuards > _guards){var path = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;SpawnAgent(true, _guardPrefabs.ElementAt(Random.Range(0, _guardPrefabs.Count)), "NavAgentGuard", "Path", path.ElementAt(Random.Range(0, path.Count)), Quaternion.identity, path);}
        while(_nPeople > _people)
        {
            var targets = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;
            targets.Add(_spawns.ElementAt(Random.Range(0, _spawns.Count)));
            GameObject prefab;
            string classname;
            if(Random.Range(0f, 1f) < 0.5f)
            {
                if(Random.Range(0f, 1f) < 0.5f){prefab = _patrizioPrefabs.ElementAt(Random.Range(0, _patrizioPrefabs.Count));classname = "NavAgentPatrizio";}
                else {prefab = _patriziaPrefabs.ElementAt(Random.Range(0, _patriziaPrefabs.Count));classname = "NavAgentPatrizia";}
            }
            else if(Random.Range(0f, 1f) < 0.6f){prefab = _mercantePrefabs.ElementAt(Random.Range(0, _mercantePrefabs.Count));classname = "NavAgentMercante";}
            else{prefab = _schiavoPrefabs.ElementAt(Random.Range(0, _schiavoPrefabs.Count));classname = "NavAgentSchiavo";}
            SpawnAgent(true, prefab, classname, "Move", _spawns.ElementAt(Random.Range(0, _spawns.Count)), Quaternion.identity, targets);
        }
    }

    private GameObject SpawnAgent(bool count, GameObject prefab, string classname, string state, Vector3 position, Quaternion rotation, List<Vector3> targets)
    {
        GameObject agent = Instantiate(prefab, position, rotation);
        switch (classname)
        {
            case "NavAgentSchiavo":
                agent.AddComponent<NavAgentSchiavo>();
                agent.GetComponent<NavAgentSchiavo>().SetState(state);
                if(targets != null && targets?.Count > 0)agent.GetComponent<NavAgentSchiavo>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentMercante":
                agent.AddComponent<NavAgentMercante>();
                agent.GetComponent<NavAgentMercante>().SetState(state);
                if(targets != null && targets?.Count > 0)agent.GetComponent<NavAgentMercante>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentPatrizio":
                agent.AddComponent<NavAgentPatrizio>();
                agent.GetComponent<NavAgentPatrizio>().SetState(state);
                if(targets != null && targets?.Count > 0)agent.GetComponent<NavAgentPatrizio>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentPatrizia":
                agent.AddComponent<NavAgentPatrizia>();
                agent.GetComponent<NavAgentPatrizia>().SetState(state);
                if(targets != null && targets?.Count > 0)agent.GetComponent<NavAgentPatrizia>().SetTargets(targets);
                if(count)_people++;
                break;
            case "NavAgentGuard":
                agent.AddComponent<NavAgentGuard>();
                agent.GetComponent<NavAgentGuard>().SetState(state);
                if(targets != null && targets?.Count > 0)agent.GetComponent<NavAgentGuard>().SetTargets(targets);
                if(count)_guards++;
                break;
            default: throw new System.ArgumentOutOfRangeException();
        }
        agent.transform.parent = gameObject.transform;
        return agent;
    }

    public void DestroyedAgent(string classType)
    {
        switch (classType)
        {
            case "NavAgentSchiavo": _people--;break;
            case "NavAgentMercante": _people--;break;
            case "NavAgentPatrizio": _people--;break;
            case "NavAgentPatrizia": _people--;break;
            case "NavAgentGuard": _guards--;break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }
}