using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class NpcMySchiavo : NpcInteractable
{
    public bool _switch = false;

    void Start()
    {
        
    }

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        if (_switch) StartCoroutine(Subtitles());
        else
        {
            SetTalkIndex(0);
            base.StartInteraction();
        }
    }

    private IEnumerator Subtitles()
    {
        var player = FindObjectOfType<InteractionManager>();
        var playerInteractionManager = player.GetComponent<InteractionManager>();
        var playerAudioSource = player.GetComponents<AudioSource>()[1];
        var playerFirstPersonController = player.GetComponent<FirstPersonController>();
        var _audioSubManager = FindObjectOfType<AudioSubManager>();

        int index = 0;

        playerFirstPersonController.enabled = false;
        playerInteractionManager.enabled = false;
        UITextOff();

        animator.SetBool(NavAgent.NavAgentStates.Talk.ToString(), true);

        SetAudio(++index);
        SetSubtitles(index);
        yield return new WaitForSeconds(_audioSource.clip.length);

        //player
        SetAudio(_audioSubManager.GetAudio(++index, Characters.MySchiavo, voice));
        SetSubtitles(_audioSubManager.GetSubs(index, Characters.MySchiavo, voice));
        yield return new WaitForSeconds((playerAudioSource.clip.length) / 2);

        SetSubtitles(_audioSubManager.GetSubs(++index, Characters.MySchiavo, voice));
        yield return new WaitForSeconds((playerAudioSource.clip.length) / 2);

        SetSubtitles("");

        //chiama animazione swoosh
        //GameObject go = Instantiate(nobile, player.transform.position, player.transform.rotation, transform) as GameObject;

        Globals.someoneIsTalking = false;
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
        playerInteractionManager.enabled = true;
        
        Destroy(gameObject);
    }
}
