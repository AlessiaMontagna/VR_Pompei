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
    [SerializeField] private List<GameObject> _nobileMPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _amicoPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _nobileFPrefabs = new List<GameObject>();
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
    public Dictionary<Characters, List<GameObject>> prefabs => _prefabs;
    private Dictionary<NavSubroles, List<GameObject>> _stops = new Dictionary<NavSubroles, List<GameObject>>();
    private Dictionary<NavSubroles, List<GameObject>> _paths = new Dictionary<NavSubroles, List<GameObject>>();
    private Dictionary<NavSubroles, List<GameObject>> _spawns = new Dictionary<NavSubroles, List<GameObject>>();
    public Dictionary<NavSubroles, List<GameObject>> navspawns => _spawns;

    void Awake()
    {
        _prefabs.Add(Characters.Guardia, _guardPrefabs);
        _prefabs.Add(Characters.Soldato, _soldierPrefabs);
        _prefabs.Add(Characters.Schiavo, _schiavoPrefabs);
        _prefabs.Add(Characters.Mercante, _mercantePrefabs);
        _prefabs.Add(Characters.NobileM, _nobileMPrefabs);
        _prefabs.Add(Characters.Amico, _amicoPrefabs);
        _prefabs.Add(Characters.NobileF, _nobileFPrefabs);
        // get STOPS PATHS and SPAWNS
        foreach (var item in GameObject.FindObjectsOfType<NavElement>().Where(i => i != null))
        {
            var navElement = item.GetComponent<NavElement>();
            switch (navElement.role)
            {
                case NavRoles.Spawn:
                    if(_spawns.TryGetValue(navElement.subrole, out var tlist)) tlist.Add(item.gameObject);
                    else _spawns.Add(navElement.subrole, new List<GameObject>{item.gameObject});
                    break;
                case NavRoles.Stop:
                    if(_stops.TryGetValue(navElement.subrole, out tlist)) tlist.Add(item.gameObject);
                    else _stops.Add(navElement.subrole, new List<GameObject>{item.gameObject});
                    break;
                case NavRoles.Path:
                    if(_paths.TryGetValue(navElement.subrole, out var vlist)) vlist.Add(item.gameObject);
                    else _paths.Add(navElement.subrole, new List<GameObject>{item.gameObject});
                    break;
                default: throw new System.ArgumentOutOfRangeException();
            }
        }
        // spawn AMICO
        if(FindObjectOfType<NpcAmico>() == null && _stops.TryGetValue(NavSubroles.AmicoStop, out var stops) && _prefabs.TryGetValue(Characters.Amico, out var prefabs))foreach (var item in stops.Where(i => i != null)){SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Amico, "Idle", item, default(Vector3), null);}
        // spawn MYSCHIAVO
        if(FindObjectOfType<NpcMySchiavo>() == null && _stops.TryGetValue(NavSubroles.MySchiavoStop, out stops) && _prefabs.TryGetValue(Characters.Schiavo, out prefabs))foreach (var item in stops.Where(i => i != null)){SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.MySchiavo, "Idle", item, default(Vector3), null);}
    }

    void Start()
    {
        List<GameObject> prefabs;
        List<GameObject> stops;
        // spawn TUTORIAL
        var tutorialManager = FindObjectOfType<TutorialManager>();
        if(tutorialManager != null && tutorialManager.enabled && _prefabs.TryGetValue(Characters.Schiavo, out prefabs))
            SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.SchiavoTutorial, "Idle", tutorialManager.gameObject, default(Vector3), null);
        // spawn STOPS guards
        if(_stops.TryGetValue(NavSubroles.GuardStop, out stops) && _prefabs.TryGetValue(Characters.Soldato, out prefabs))foreach (var item in stops.Where(i => i != null)){SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Guardia, "Idle", item, default(Vector3), null);}
        // spawn STOPS soldier
        if(_stops.TryGetValue(NavSubroles.SoldierStop, out stops) && _prefabs.TryGetValue(Characters.Soldato, out prefabs))foreach (var item in stops.Where(i => i != null)){SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Soldato, "Idle", item, default(Vector3), null);}
        // spawn STOPS mercanti
        if(_stops.TryGetValue(NavSubroles.MercanteStop, out stops) && _prefabs.TryGetValue(Characters.Mercante, out prefabs))foreach(var item in stops.Where(i => i != null)){SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Mercante, "Idle", item, default(Vector3), null);}
        // spawn STOPS balcony
        if(_stops.TryGetValue(NavSubroles.BalconyStop, out stops)) foreach (var item in stops.Where(i => i != null))
        {
            // TODO: POSE
            Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character != Characters.NobileM && character != Characters.NobileF);
            if(!_prefabs.TryGetValue(character, out prefabs))Debug.LogError("PREFABS ERROR");
            SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Idle", item, default(Vector3), null);
        }
        // STOPS groups
        if(_stops.TryGetValue(NavSubroles.GroupStop, out stops)) foreach (var item in stops.Where(i => i != null))
        {
            int count = Random.Range(2, 5);
            List<Vector2> randoms = new List<Vector2>();
            for (int i = 0; i < count; i++)
            {
                Vector2 random;
                bool used = false;
                do{random = Random.insideUnitCircle.normalized * 1f;foreach (var rand in randoms){if(rand.x-random.x<5f || rand.y-random.y<5f)used = true;}}while(used && !UnityEngine.AI.NavMesh.SamplePosition(item.transform.position + new Vector3(random.x, 0, random.y), out UnityEngine.AI.NavMeshHit hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas));
                randoms.Add(random);
                Vector3 position = item.transform.position + new Vector3(random.x, 0, random.y);
                Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character != Characters.Guardia && character != Characters.Mercante && character != Characters.NobileM && character != Characters.NobileF);
                if(!_prefabs.TryGetValue(character, out prefabs))Debug.LogError("PREFABS ERROR");
                var agent = SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Talk", item, position, null);
            }
        }
        // SET agents total numbers
        if(_nPeople < 0 || _nPeople > MAXPEOPLE) _nPeople = Random.Range(10, MAXPEOPLE+1);
        if(_nGuards < 0 || _nGuards > MAXGUARDS) _nGuards = Random.Range(5, MAXGUARDS+1);
        if(_nFlocks < 0 || _nFlocks > MAXFLOCKS) _nFlocks = Random.Range(5, MAXFLOCKS+1);
        int flocksToSpawn = _nFlocks;
        // spawn BIRDS
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
        // spawn people
        List<Vector3> path = new List<Vector3>();
        List<GameObject> pathGO = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;
        foreach(var item in pathGO){path.Add(item.transform.position);}
        if(!_prefabs.TryGetValue(Characters.Guardia, out prefabs))Debug.LogError("PREFABS ERROR");
    }

    void Update()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "ScenaLapilli")return;
        // SPAWN agents if there are less then defined in Start()
        while(_nGuards > _guards)
        {
            List<Vector3> path = new List<Vector3>();
            List<GameObject> pathGO = _paths.ElementAt(Random.Range(0, _paths.Count)).Value;
            foreach(var item in pathGO){path.Add(item.transform.position);}
            if(!_prefabs.TryGetValue(Characters.Guardia, out var prefabs))Debug.LogError("PREFABS ERROR");
            SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), Characters.Guardia, "Path", pathGO.ElementAt(Random.Range(0, pathGO.Count)), default(Vector3), path);}
        while(_nPeople > _people)
        {
            if(Random.Range(0f, 1f) < 0.2)
            {
                if(!_spawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("SPAWN ERROR");
                spawns = spawns.Where(i => i.transform.position.y > 5f).ToList();
                Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Guardia || character == Characters.Soldato);
                if(!_prefabs.TryGetValue(character, out var prefabs))Debug.LogError("PREFABS ERROR");
                var spawn = spawns.ElementAt(Random.Range(0, spawns.Count));
                spawns.Remove(spawn);
                SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Move", spawn, default(Vector3), new List<Vector3>{spawns.ElementAt(Random.Range(0, spawns.Count)).transform.position});
            }
            else
            {
                List<Vector3> path = new List<Vector3>();
                foreach(var item in _paths.ElementAt(Random.Range(0, _paths.Count)).Value){path.Add(item.transform.position);}
                if(!_spawns.TryGetValue(NavSubroles.PeopleSpawn, out var spawns))Debug.LogError("SPAWN ERROR");
                path.Add(spawns.ElementAt(Random.Range(0, spawns.Count)).transform.position);
                Characters character;do{character = _prefabs.Keys.ElementAt(Random.Range(0, _prefabs.Keys.Count));}while(character == Characters.Guardia || character == Characters.Soldato);
                if(!_prefabs.TryGetValue(character, out var prefabs))Debug.LogError("PREFABS ERROR");
                SpawnAgent(prefabs.ElementAt(Random.Range(0, prefabs.Count)), character, "Move", spawns.ElementAt(Random.Range(0, spawns.Count)), default(Vector3), path);
            }
        }
    }

    public GameObject SpawnAgent(GameObject prefab, Characters character, string state, GameObject parent, Vector3 position, List<Vector3> targets)
    {
        if(position == default(Vector3))position = parent.transform.position - parent.transform.forward * 0.6f;
        GameObject agent = Instantiate(prefab, position, parent.transform.rotation);
        agent.transform.parent = parent.transform;
        NpcInteractable component;
        switch(character)
        {
            case Characters.Mercante: if(parent.GetComponent<NavElement>().foodType != MercanteFoodTypes.None){component = agent.AddComponent<NpcMercante>();break;}goto default;
            case Characters.SchiavoTutorial: component = agent.AddComponent<NpcTutorial>();break;
            case Characters.MySchiavo: component = agent.AddComponent<NpcMySchiavo>();break;
            case Characters.Amico: component = agent.AddComponent<NpcAmico>();break;
            case Characters.Soldato: component = agent.AddComponent<NpcSoldato>();break;
            default: component = agent.AddComponent<NpcInteractable>();break;
        }
        component.Initialize(character, parent, state, targets);
        if(targets != null && targets?.Count > 0)
        {
            if(character == Characters.Guardia)_guards++;else _people++;
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
        return agent;
    }

    public void DestroyedAgent(Characters character){if(character == Characters.Guardia){_guards++;}else{_people++;}}
}