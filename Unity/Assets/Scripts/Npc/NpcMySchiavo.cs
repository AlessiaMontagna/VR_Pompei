using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class NpcMySchiavo : NpcInteractable
{
    [SerializeField] private GameObject nobile;

    public bool _switch = false;

    void Start()
    {
        if(navAgent != null)return;
        Initialize(Characters.MySchiavo, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);
    }

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        if(_switch) StartCoroutine(MissionTalk(0));
        else base.StartInteraction();
    }

    private IEnumerator MissionTalk(int index)
    {
        var player = FindObjectOfType<InteractionManager>();
        var playerFirstPersonController = player.GetComponent<FirstPersonController>();

        playerFirstPersonController.enabled = false;
        player.GetComponent<InteractionManager>().enabled = false;
        UITextOff();
        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);

        SetAudio(++index);
        SetSubtitles(index);
        yield return new WaitForSeconds(GetAudioLength());

        //AUDIO SOURCE DA SOSTITUIRE
        var playerAudioSource = player.GetComponents<AudioSource>()[1];
        playerAudioSource.clip = Resources.Load<AudioClip>(audioSubManager.GetAudio(++index, Characters.MySchiavo, voice));
        playerAudioSource.Play();
        //AUDIO SOURCE DA SOSTITUIRE
        
        SetSubtitles(index);
        yield return new WaitForSeconds(GetAudioLength() / 2f);
        SetSubtitles(++index);
        yield return new WaitForSeconds(GetAudioLength() / 2f);

        // Swoosh personaggi
        // GameObject go = Instantiate(nobile, player.transform.position, player.transform.rotation, transform) as GameObject;
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
