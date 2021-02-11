using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NavSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _guardPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _soldierPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _schiavoPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _mercantePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _patrizioPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _patriziaPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _birdsPrefabs = new List<GameObject>();
    [SerializeField] private int _nGuards;
    [SerializeField] private int _nPeople;
    [SerializeField] private int _nFlocks;

    private int _people = 0;
    private int _guards = 0;

    readonly int MAXPEOPLE = 15;
    readonly int MAXGUARDS = 8;
    readonly int MAXFLOCKS = 8;
    readonly int MAXBIRDSPERFLOCK = 5;

    private Dictionary<Characters, List<GameObject>> _prefabs = new Dictionary<Characters, List<GameObject>>();
    private Dictionary<NavSubroles, List<Transform>> _stops = new Dictionary<NavSubroles, List<Transform>>();
    private Dictionary<NavSubroles, List<Vector3>> _paths = new Dictionary<NavSubroles, List<Vector3>>();
    private Dictionary<NavSubroles, List<Transform>> _spawns = new Dictionary<NavSubroles, List<Transform>>();

    void Start()
    {
        _prefabs.Add(Characters.Guardia, _guardPrefabs);
        _prefabs.Add(Characters.Soldato, _soldierPrefabs);
        _prefabs.Add(Characters.Schiavo, _schiavoPrefabs);
        _prefabs.Add(Characters.Mercante, _mercantePrefabs);
        _prefabs.Add(Characters.NobileM, _patrizioPrefabs);
        _prefabs.Add(Characters.NobileF, _patriziaPrefabs);
        // get STOPS PATHS and SPAWNS
        foreach (var item in GameObject.FindObjectsOfType<NavElement>().Where(i => i != null))
        {
            var component = item.GetComponent<NavElement>();
            switch (component.GetRole())
            {
                case NavRoles.Spawn:
                    if(_spawns.TryGetValue(component.GetSubrole(), out var tlist)) tlist.Add(item.transform);
                    else _spawns.Add(component.GetSubrole(), new List<Transform>{item.transform});
                    break;
                case NavRoles.Stop:
                    if(_stops.TryGetValue(component.GetSubrole(), out tlist)) tlist.Add(item.transform);
                    else _stops.Add(component.GetSubrole(), new List<Transform>{item.transform});
                    break;
                case NavRoles.Path:
                    if(_paths.TryGetValue(component.GetSubrole(), out var vlist)) vlist.Add(new Vector3(item.transform.position.x, 0, item.transform.position.z));
                    else _paths.Add(component.GetSubrole(), new List<Vector3>{new Vector3(item.transform.position.x, 0, item.transform.position.z)});
                    break;
                default: throw new System.ArgumentOutOfRangeException();
            }
        }
        // spawn STOPS guards
        if(_stops.TryGetValue(NavSubroles.GuardStop, out var stops) && _prefabs.TryGetValue(Characters.Soldato, out var prefabs))
        foreach (var item in stops.Where(i => i != null)){var agent = SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Guardia, "Idle", item.position, item.rotation, null);agent.GetComponent<CapsuleCollider>().radius = 1.5f;}
        // spawn STOPS mercanti
        if(_stops.TryGetValue(NavSubroles.MercanteStop, out stops) && _prefabs.TryGetValue(Characters.Mercante, out prefabs))foreach(var item in stops.Where(i => i != null)){var agent = SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Mercante, "Idle", item.position, item.rotation, null);agent.GetComponent<CapsuleCollider>().radius = 1.5f;}
        // spawn STOPS balcony
        if(_stops.TryGetValue(NavSubroles.BalconyStop, out stops)) foreach (var item in stops.Where(i => i != null))
        {
            // TODO: POSE
            Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Guardia || character == Characters.Soldato || character == Characters.Schiavo);
            if(!_prefabs.TryGetValue(character, out prefabs))Debug.LogError("PREFABS ERROR");
            SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Idle", item.position, item.rotation, null);
        }
        // STOPS groups
        if(_stops.TryGetValue(NavSubroles.GroupStop, out stops)) foreach (var item in stops.Where(i => i != null))
        {
            Vector3 position;
            Quaternion rotation;
            int count = Random.Range(2, 5);
            for (int i = 0; i < count; i++)
            {
                do{Vector2 random = Random.insideUnitCircle.normalized * Random.Range(1f, 1.1f);position = item.position + new Vector3(random.x, 0, random.y);}
                while(!UnityEngine.AI.NavMesh.SamplePosition(position, out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas));
                rotation = Quaternion.LookRotation(item.position-position, Vector3.up);
                Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Soldato || character == Characters.Schiavo);
                if(!_prefabs.TryGetValue(character, out prefabs))Debug.LogError("PREFABS ERROR");
                var agent = SpawnAgent(false, prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Talk", position, rotation, null);
                agent.GetComponent<CapsuleCollider>().radius = 1.3f;
            }
        }
        // SET agents total numbers
        if(_nPeople < 0 || _nPeople > MAXPEOPLE) _nPeople = Random.Range(10, MAXPEOPLE+1);
        if(_nGuards < 0 || _nGuards > MAXGUARDS) _nGuards = Random.Range(5, MAXGUARDS+1);
        if(_nFlocks < 0 || _nFlocks > MAXFLOCKS) _nFlocks = Random.Range(5, MAXFLOCKS+1);
        int flocksToSpawn = _nFlocks;
        if(_spawns.TryGetValue(NavSubroles.FlocksSpawn, out var spawns)) foreach (var item in spawns.Where(i => i != null))
        {
            if(flocksToSpawn <= 0) break;
            int nFlocks = Random.Range(1, flocksToSpawn);
            flocksToSpawn -= nFlocks;
            var prefab = _birdsPrefabs.ElementAt(Random.Range(0, _birdsPrefabs.Count));
            GameObject flockFlyingTarget = Instantiate(prefab, item.transform.position, Quaternion.identity);
            flockFlyingTarget.transform.parent = item.transform;
            int nBirds = Random.Range(5, MAXBIRDSPERFLOCK);
            for (int j = 0; j < nBirds; j++)
            {
                GameObject bird = Instantiate(prefab, item.transform.position + Random.insideUnitSphere * 5, Quaternion.identity);
                bird.transform.parent = item.transform;
                bird.GetComponent<RandomFlyer>().SetFlyingTarget(flockFlyingTarget);
            }
        }
    }

    void Update()
    {
        // SPAWN agents if there are less then defined in Start()
        while(_nGuards > _guards){var path = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;if(!_prefabs.TryGetValue(Characters.Guardia, out var prefabs))Debug.LogError("PREFABS ERROR");SpawnAgent(true, prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Guardia, "Path", path.ElementAt(Random.Range(0, path.Count)), Quaternion.identity, path);}
        while(_nPeople > _people)
        {
            var targets = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;
            if(!_spawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("PREFABS ERROR");
            targets.Add(spawns.ElementAt(Random.Range(0, spawns.Count)).position);
            Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Guardia);
            if(!_prefabs.TryGetValue(character, out var prefabs))Debug.LogError("PREFABS ERROR");
            SpawnAgent(true, prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Move", spawns.ElementAt(Random.Range(0, spawns.Count)).position, Quaternion.identity, targets);
        }
    }

    private GameObject SpawnAgent(bool count, GameObject prefab, Characters character, string state, Vector3 position, Quaternion rotation, List<Vector3> targets)
    {
        GameObject agent = Instantiate(prefab, position, rotation);
        agent.transform.parent = gameObject.transform;
        if(count){if(character == Characters.Guardia){_guards++;}else{_people++;}}
        if(state == "Move" || state == "Path")
        {
            agent.AddComponent<Rigidbody>();
            var body = agent.GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            body.mass = 100f;
            body.drag = 0.01f;
            body.angularDrag = 0.05f;
            body.useGravity = true;
            body.interpolation = RigidbodyInterpolation.None;
            body.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
        var component = agent.AddComponent<Npc>();
        component.SetCharacter(character);
        component.SetRotation(rotation);
        component.SetState(state);
        if(targets != null && targets?.Count > 0)component.SetTargets(targets);
        return agent;
    }

    public void DestroyedAgent(Characters character){if(character == Characters.Guardia){_guards++;}else{_people++;}}
}