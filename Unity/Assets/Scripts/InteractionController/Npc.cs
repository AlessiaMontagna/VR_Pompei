using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Npc : MonoBehaviour
{
    private Text dialogueText;
    private Characters _characterType;

    private AudioSource audioSource;
    private AudioSubManager _subtitles;
    private string[] vociMaschili = { "Giorgio", "Francesco", "Antonio", "Klajdi", "Edoardo", "Fabrizio", "Andrea", "Diego" };
    private string[] vociFemminili = { "Alessia", "Sofia", "Nadia", "Paola", "Stella" };
    private string voce;
    System.Random rnd;
    private int maxRand;
    private string path;
    private NpcInteractable npc;


    void Start()
    {
        dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        _subtitles = FindObjectOfType<AudioSubManager>();
        npc = GetComponent<NpcInteractable>();
        rnd = new System.Random();
        //switch (_characterType)
        //{
        //    case Characters.NobileM:
        //        maxRand = 2;
        //        voce = vociMaschili[rnd.Next(0, vociMaschili.Length)];
        //        break;
        //    case "NobileF":
        //        maxRand = 3;
        //        voce = vociFemminili[rnd.Next(0, vociFemminili.Length)];
        //        break;
        //    case "Guardia":
        //        maxRand = 3;
        //        voce = vociMaschili[rnd.Next(0, vociMaschili.Length)];
        //        break;
        //    case "Mercante":
        //        maxRand = 5;
        //        voce = vociMaschili[rnd.Next(0, vociMaschili.Length)];
        //        break;
        //}
        audioSource = GetComponent<AudioSource>();
    }

    public void Talk(GameObject caller)
    {
        
        int index = rnd.Next(1, maxRand);
        if (!audioSource.isPlaying)
        {
            //switch (caller.tag)
            //{
            //    case "Player_Nobile":
            //        path = "Nobile_" + _characterType + index;
            //        audioSource.clip = Resources.Load<AudioClip>("Talking/" + path + "_" + voce);
            //        audioSource.Play();
            //        StartCoroutine(SubtitlesNobile(index));
            //        break;
            //    case "Player_Schiavo":
            //        path = "Schiavo_" + _characterType + index;
            //        audioSource.clip = Resources.Load<AudioClip>("Talking/" + path + "_" + voce);
            //        audioSource.Play();
            //        StartCoroutine(SubtitlesSchiavo(index));
            //        break;

            //}
            
        }
    }

    //private IEnumerator SubtitlesNobile(int i)
    //{
    //    npc.isTalking = true;
    //    dialogueText.text = _subtitles.GetSubs_PlayerNobile(i, _characterType);
    //    yield return new WaitForSeconds(audioSource.clip.length);
    //    npc.isTalking = false;
    //    dialogueText.text = "";
    //}

    //private IEnumerator SubtitlesSchiavo(int i)
    //{
    //    npc.isTalking = true;
    //    dialogueText.text = _subtitles.GetSubs_PlayerSchiavo(i, _characterType);
    //    yield return new WaitForSeconds(audioSource.clip.length);
    //    npc.isTalking = false;
    //    dialogueText.text = "";
    //}
}
