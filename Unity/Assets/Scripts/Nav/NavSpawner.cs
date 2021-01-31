using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum Characters{Guard, Schiavo, Mercante, Patrizio, Patrizia};

public class NavSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _guardPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _schiavoPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _mercantePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _patrizioPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _patriziaPrefabs = new List<GameObject>();
    [SerializeField] private int _nGuards;
    [SerializeField] private int _nPeople;
    
    private int _people = 0;
    private int _guards = 0;
    private readonly int MAXPEOPLE = 15;
    private readonly int MAXGUARDS = 7;

    private Dictionary<Characters, List<GameObject>> _prefabs = new Dictionary<Characters, List<GameObject>>();
    private Dictionary<string, List<Transform>> _stops = new Dictionary<string, List<Transform>>();
    private Dictionary<string, List<Vector3>> _paths = new Dictionary<string, List<Vector3>>();
    private List<Vector3> _spawns = new List<Vector3>();

    void Start()
    {
        _prefabs.Add(Characters.Guard, _guardPrefabs);
        _prefabs.Add(Characters.Schiavo, _schiavoPrefabs);
        _prefabs.Add(Characters.Mercante, _mercantePrefabs);
        _prefabs.Add(Characters.Patrizio, _patrizioPrefabs);
        _prefabs.Add(Characters.Patrizia, _patriziaPrefabs);
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
        if(!_stops.TryGetValue("GuardStop", out var stops) || !_prefabs.TryGetValue(Characters.Guard, out var prefabs))Debug.LogError("GUARD PREFAB OR STOPS ERROR");
        else foreach (var item in stops.Where(i => i != null)){SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Guard, "Idle", item.position, item.rotation, null);}
        // spawn STOPS mercanti
        if(!_stops.TryGetValue("MercanteStop", out stops) || !_prefabs.TryGetValue(Characters.Mercante, out prefabs))Debug.LogError("MERCANTI PREFAB OR STOPS ERROR");
        else foreach (var item in stops.Where(i => i != null)){SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Mercante, "Idle", item.position, item.rotation, null);}
        // spawn STOPS balcony
        if(!_stops.TryGetValue("BalconyStop", out stops)) Debug.LogError("BALCONY PREFAB OR STOPS ERROR");
        else foreach (var item in stops.Where(i => i != null))
        {
            // TODO: POSE
            Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Guard || character == Characters.Schiavo);
            if(!_prefabs.TryGetValue(character, out prefabs))Debug.LogError("PREFABS ERROR");
            SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Idle", item.position, item.rotation, null);
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
                Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Guard || character == Characters.Schiavo);
                if(!_prefabs.TryGetValue(character, out prefabs))Debug.LogError("PREFABS ERROR");
                SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Talk", position, rotation, null);
            }
        }
        // SET agents total numbers
        if(_nPeople < 0 || _nPeople > MAXPEOPLE) _nPeople = Random.Range(7, MAXPEOPLE+1);
        if(_nGuards < 0 || _nGuards > MAXGUARDS) _nGuards = Random.Range(2, MAXGUARDS+1);
    }

    void Update()
    {
        // SPAWN agents if there are less then defined in Start()
        while(_nGuards > _guards){var path = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;if(!_prefabs.TryGetValue(Characters.Guard, out var prefabs))Debug.LogError("PREFABS ERROR");SpawnAgent(true, prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Guard, "Path", path.ElementAt(Random.Range(0, path.Count)), Quaternion.identity, path);}
        while(_nPeople > _people)
        {
            var targets = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;
            targets.Add(_spawns.ElementAt(Random.Range(0, _spawns.Count)));
            Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Guard);
            if(!_prefabs.TryGetValue(character, out var prefabs))Debug.LogError("PREFABS ERROR");
            SpawnAgent(true, prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Move", _spawns.ElementAt(Random.Range(0, _spawns.Count)), Quaternion.identity, targets);
        }
    }

    private GameObject SpawnAgent(bool count, GameObject prefab, Characters character, string state, Vector3 position, Quaternion rotation, List<Vector3> targets)
    {
        GameObject agent = Instantiate(prefab, position, rotation);
        agent.transform.parent = gameObject.transform;
        if(count){if(character == Characters.Guard){_guards++;}else{_people++;}}
        var component = agent.AddComponent<Npc>();
        component.SetCharacter(character);
        component.SetState(state);
        if(targets != null && targets?.Count > 0)component.SetTargets(targets);
        return agent;
    }

    public void DestroyedAgent(Characters character){if(character == Characters.Guard){_guards++;}else{_people++;}}
}