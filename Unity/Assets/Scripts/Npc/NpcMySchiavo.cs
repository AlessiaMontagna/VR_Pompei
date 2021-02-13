using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NpcMySchiavo : NpcInteractable
{
    

    void Start()
    {
        _missionManager = FindObjectOfType<MissionManager>();
        _audioSubManager = FindObjectOfType<AudioSubManager>();
        _dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
    }

    protected override void StartInteraction()
    {
        Globals.someoneIsTalking = true;
        if (_switch) StartCoroutine(Subtitles());
        else base.StartInteraction();
    }

    protected override void UpdateInteraction()
    {
        base.UpdateInteraction();
    }

    protected override void StopInteraction()
    {
        base.StopInteraction();
    }

    private IEnumerator Subtitles(int index)
    {
        var player = FindObjectOfType<InteractionManager>();
        var playerInteractionManager = player.GetComponent<InteractionManager>();
        var playerAudioSource = player.GetComponents<AudioSource>()[1];
        var playerFirstPersonController = player.GetComponent<FirstPersonController>();

        playerFirstPersonController.enabled = false;
        playerInteractionManager.enabled = false;
        UITextOff();
        
        path = "Talking/MySchiavo/";
        audioSource.clip = Resources.Load<AudioClip>(path + index.ToString());
        audioSource.Play();
        _dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds(_audioSource.clip.length);
        playerAudioSource.clip = Resources.Load<AudioClip>(path + index.ToString());
        playerAudioSource.Play();
        _dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds((playerAudioSource.clip.length) / 2);
        _dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds((playerAudioSource.clip.length) / 2);
        _dialogueText.text = "";
        //chiama animazione swoosh
        //GameObject go = Instantiate(nobile, player.transform.position, player.transform.rotation, transform) as GameObject;
        Globals.someoneIsTalking = false;
        Globals.player = Players.Schiavo;

        GetComponent<CapsuleCollider>().enabled = false;
        _mission.UpdateMission(Missions.Mission3_GetFood);
        playerFirstPersonController.teleporting = true;
        yield return new WaitForEndOfFrame();
        player.transform.rotation = transform.rotation;
        Vector3 newPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.transform.position = newPosition;
        playerFirstPersonController.InitMouseLook(player.transform);
        yield return new WaitForFixedUpdate();
        playerFirstPersonController.teleporting = false;
        playerFirstPersonController.enabled = true;
        playerInteractionManager.enabled = true;

        Destroy(gameObject);
    }
}
