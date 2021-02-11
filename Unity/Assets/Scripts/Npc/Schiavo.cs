using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class Schiavo : MonoBehaviour
{
    [SerializeField] private GameObject fpc2;
    [SerializeField] private GameObject nobile;
    private AudioSubManager _sottotitoli;
    
    private AudioSource audioSource;
    private Text dialogueText;
    private int index = 0;
    private string path;
    private void Start()
    {
        _sottotitoli = FindObjectOfType<AudioSubManager>();
        audioSource = GetComponent<AudioSource>();
        dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
    }
    public void Talk(bool _switch, GameObject caller)
    {
        if (!audioSource.isPlaying)
        {
            GetComponent<MySchiavoInteractable>()._isTalking = true;
            if (_switch)
            {
                index = 1;
                AudioSource playerAudio = caller.GetComponents<AudioSource>()[1];
                StartCoroutine(Subtitles(caller, playerAudio));
            }
            else
            {
                audioSource.clip = Resources.Load<AudioClip>("Talking/Nobile_Schiavo" + index.ToString());
                audioSource.Play();
                StartCoroutine(StaticDialogue(audioSource.clip));
            }
        }
            
    }

    private IEnumerator Subtitles(GameObject player, AudioSource playerAudio)
    {
        player.GetComponent<FirstPersonController>().enabled = false;
        player.GetComponent<InteractionManager>().enabled = false;
        GetComponent<MySchiavoInteractable>().UITextOff();
        path = "Talking/Nobile_Schiavo";
        audioSource.clip = Resources.Load<AudioClip>(path + index.ToString());
        audioSource.Play();
        dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds(audioSource.clip.length);
        playerAudio.clip = Resources.Load<AudioClip>(path + index.ToString());
        playerAudio.Play();
        dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds((playerAudio.clip.length) / 2);
        dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        index++;
        yield return new WaitForSeconds((playerAudio.clip.length) / 2);
        dialogueText.text = "";
        //chiama animazione swoosh
        //GameObject go = Instantiate(nobile, player.transform.position, player.transform.rotation, transform) as GameObject;
        

        
    }
    public void Switch(GameObject player)
    {
        Destroy(gameObject);
        player.SetActive(false);
        fpc2.SetActive(true);
        Globals.player = Players.Schiavo;
    }

    private IEnumerator StaticDialogue(AudioClip clip)
    {
        dialogueText.text = _sottotitoli.GetSubs(index, Characters.MySchiavo);
        yield return new WaitForSeconds(clip.length);
        dialogueText.text = "";
        GetComponent<MySchiavoInteractable>()._isTalking = false;
    }
    
}
