﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class NpcMySchiavo : NpcInteractable
{
    [SerializeField] private GameObject nobile;

    public bool swoosh = false;
    private LevelChangerScript _swoosh;

    // Start is called before the first frame update
    void Start()
    {
        _swoosh = FindObjectOfType<LevelChangerScript>();
        if(navAgent != null) return;
        Initialize(Characters.MySchiavo, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);
        gameObject.transform.parent = FindObjectsOfType<NavElement>().Where(i => i != null && i.subrole == NavSubroles.MySchiavoStop).ElementAt(0).gameObject.transform;
    }

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        if(swoosh) StartCoroutine(Mission3Talk(0));
        else StartCoroutine(Talk(0));
    }

    private IEnumerator Mission3Talk(int index)
    {
        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);
        var player = FindObjectOfType<InteractionManager>();
        var playerFirstPersonController = player.GetComponent<FirstPersonController>();

        playerFirstPersonController.enabled = false;
        player.GetComponent<InteractionManager>().enabled = false;
        FindObjectOfType<ShowAgenda>().enabled = false;
        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);

        SetAudio(++index);
        SetSubtitles(index);
        yield return new WaitForSeconds(GetAudioLength());

        var playerAudioSource = player.GetComponents<AudioSource>()[1];
        playerAudioSource.clip = Resources.Load<AudioClip>(audioSubManager.GetAudio(++index, Characters.MySchiavo, voice));
        playerAudioSource.Play();
        
        SetSubtitles(index);
        yield return new WaitForSeconds(GetAudioLength());
        playerAudioSource.clip = Resources.Load<AudioClip>(audioSubManager.GetAudio(++index, Characters.MySchiavo, voice));
        playerAudioSource.Play();
        SetSubtitles(index);
        yield return new WaitForSeconds(GetAudioLength());
        _swoosh.SwooshIn();
        yield return new WaitForSeconds(0.5f);
        
        var playerPosition = player.gameObject.transform.position;
        var playerRotation = player.gameObject.transform.rotation;
        var spawner = FindObjectOfType<NavSpawner>();
        if(!spawner.stops.TryGetValue(NavSubroles.AmicoStop, out var stop))Debug.LogWarning("Amico stop not found");
        var parent = stop.FirstOrDefault();
        if(!spawner.prefabs.TryGetValue(Characters.Amico, out var prefabs))Debug.LogError("PREFAB ERROR");
        var nobile = spawner.SpawnAgent(prefabs.ElementAt(0), Characters.NobileM, "Idle", parent, playerPosition, new List<Vector3>{parent.transform.position});
        if(!nobile.GetComponent<NpcInteractable>().ChangeVoice("Edoardo"))Debug.LogWarning("'Edoardo' was NOT set as nobile voice");
        parent.transform.LookAt(nobile.transform);
        nobile.transform.position = playerPosition;
        nobile.transform.rotation = playerRotation;
        nobile.GetComponent<NpcInteractable>().WaitForMotion(5f);

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
        FindObjectOfType<ShowAgenda>().enabled = true;
        StopInteraction();
        if(nobile.GetComponent<NpcInteractable>().voice != "Edoardo")Debug.LogWarning("'Edoardo' was NOT set as nobile voice");
        Destroy(gameObject);
    }
}
