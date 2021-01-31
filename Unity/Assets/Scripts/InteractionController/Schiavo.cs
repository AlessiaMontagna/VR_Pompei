using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Schiavo : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject fpc2;
    [SerializeField] private GameObject nobile;

    public void Talk(bool _switch, GameObject caller)
    {
        if (!audioSource.isPlaying)
        {
            if (_switch)
            {
                GetComponent<MySchiavoInteractable>()._isTalking = true;
                AudioSource playerAudio = caller.GetComponents<AudioSource>()[1];
                StartCoroutine(StartDialogue(playerAudio, caller));

            }
            else
            {
                GetComponent<MySchiavoInteractable>()._isTalking = true;
                audioSource.clip = Resources.Load<AudioClip>("Talking/SchiavoStaticDialogue");
                audioSource.Play();
                StartCoroutine(StaticDialogue(audioSource.clip));
            }
        }
            
    }

    private IEnumerator StaticDialogue(AudioClip clip)
    {
        dialogueText.text = "Si capo";
        yield return new WaitForSeconds(clip.length);
        dialogueText.text = "";
        GetComponent<MySchiavoInteractable>()._isTalking = false;
    }
    private IEnumerator StartDialogue(AudioSource playerAudio, GameObject player)
    {
        player.GetComponent<FirstPersonController>().enabled = false;
        dialogueText.text = "Schiavo: Buongiorno mio padrone, ha qualche richiesta da farmi?";
        audioSource.clip = Resources.Load<AudioClip>("Talking/SchiavoToNobile");
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        dialogueText.text = "Nobile: Vai al macellum a prendermi del pesce";
        playerAudio.clip = Resources.Load<AudioClip>("Talking/NobileToSchiavo");
        playerAudio.Play();
        yield return new WaitForSeconds(playerAudio.clip.length);
        dialogueText.text = "";
        player.SetActive(false);
        fpc2.SetActive(true);
        GameObject go = Instantiate(nobile, player.transform.position, player.transform.rotation, transform) as GameObject;
        Destroy(gameObject);
        
    }
}
