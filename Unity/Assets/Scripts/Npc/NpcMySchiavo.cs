using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class NpcMySchiavo : NpcInteractable
{
    [SerializeField] private GameObject nobile;

    public bool swoosh = false;

    // Start is called before the first frame update
    void Start(){if(navAgent == null) Initialize(Characters.MySchiavo, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);}

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        if(swoosh) StartCoroutine(Mission3Talk(0));
        else base.StartInteraction();
    }

    private IEnumerator Mission3Talk(int index)
    {
        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        var player = FindObjectOfType<InteractionManager>();
        var playerFirstPersonController = player.GetComponent<FirstPersonController>();

        playerFirstPersonController.enabled = false;
        player.GetComponent<InteractionManager>().enabled = false;
        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);

        SetAudio(++index);
        SetSubtitles(index);
        yield return new WaitForSeconds(GetAudioLength());

        var playerAudioSource = player.GetComponents<AudioSource>()[1];
        playerAudioSource.clip = Resources.Load<AudioClip>(audioSubManager.GetAudio(++index, Characters.MySchiavo, voice));
        playerAudioSource.Play();
        
        SetSubtitles(index);
        yield return new WaitForSeconds(GetAudioLength() / 2f);
        SetSubtitles(++index);
        yield return new WaitForSeconds(GetAudioLength() / 2f);

        
        var playerPosition = player.gameObject.transform.position;
        var playerRotation = player.gameObject.transform.rotation;
        var spawner = FindObjectOfType<NavSpawner>();
        var parent = GameObject.FindObjectsOfType<NavElement>().ToList().Where(i => i != null && i.GetComponent<NavElement>().subrole == NavSubroles.AmicoStop).ElementAt(0).gameObject;
        if(!spawner.prefabs.TryGetValue(Characters.Amico, out var prefabs))Debug.LogError("PREFAB ERROR");
        var nobile = spawner.SpawnAgent(prefabs.ElementAt(0), Characters.NobileM, "Idle", parent, playerPosition, new List<Vector3>{parent.transform.position});
        parent.transform.LookAt(nobile.transform);
        nobile.transform.position = playerPosition;
        nobile.transform.rotation = playerRotation;
        nobile.GetComponent<NpcInteractable>().WaitForMotion(2f);

        // Swoosh personaggi
        Globals.player = Players.Schiavo;
        GetComponent<CapsuleCollider>().enabled = false;
        FindObjectOfType<MissionManager>().UpdateMission(Missions.Mission3_GetFood);
        playerFirstPersonController.teleporting = true;
        yield return new WaitForEndOfFrame();
        player.transform.rotation = transform.rotation;
        player.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        playerFirstPersonController.InitMouseLook(player.transform);
        yield return new WaitForFixedUpdate();
        playerFirstPersonController.teleporting = false;
        playerFirstPersonController.enabled = true;
        player.GetComponent<InteractionManager>().enabled = true;
        StopInteraction();
        Destroy(gameObject);
    }
}
