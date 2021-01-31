using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Npc : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Text dialogueText;
    [SerializeField] private string _characterType;
    private string[] vociMaschili = { "Giorgio", "Francesco", "Antonio", "Klajdi", "Edoardo", "Fabrizio", "Andrea", "Diego" };
    private string[] vociFemminili = { "Alessia", "Sofia", "Nadia", "Paola", "Stella" };
    private string voce;
    System.Random rnd;
    private int maxRand;
    private string path;
    private NpcInteractable npc;

    void Start()
    {
        npc = GetComponent<NpcInteractable>();
        rnd = new System.Random();
        
        //if (_characterType != "NobileM" || _characterType != "NobileF" || _characterType != "Mercante" || _characterType != "Guardia")
        //{
        //    Debug.LogError("Inserisci un nome valido: NobileM o NobileF o Mercante o Guardia");
        //}

        switch (_characterType)
        {
            case "NobileM":
                maxRand = 2;
                voce = vociMaschili[rnd.Next(0, vociMaschili.Length)];
                break;
            case "NobileF":
                maxRand = 3;
                voce = vociFemminili[rnd.Next(0, vociFemminili.Length)];
                break;
            case "Guardia":
                maxRand = 3;
                voce = "Alessia";
                //voce = vociMaschili[rnd.Next(0, vociMaschili.Length)];
                break;
            case "Mercante":
                maxRand = 5;
                voce = vociMaschili[rnd.Next(0, vociMaschili.Length)];
                break;
        }
        Debug.Log(voce);
        audioSource = GetComponent<AudioSource>();
    }

    public void Talk(GameObject caller)
    {
        
        int index = rnd.Next(1, maxRand);
        if (!audioSource.isPlaying)
        {
            switch (caller.tag)
            {
                case "Player_Nobile":
                    path = "Nobile_" + _characterType + index;
                    audioSource.clip = Resources.Load<AudioClip>("Talking/" + path + "_" + voce);
                    audioSource.Play();
                    break;
                case "Player_Schiavo":
                    path = "Schiavo_" + _characterType + index;
                    audioSource.clip = Resources.Load<AudioClip>("Talking/Schiavo_" + path + "_" + voce);
                    audioSource.Play();
                    break;

            }
            StartCoroutine(Subtitles(path));
        }
    }

    private IEnumerator Subtitles(string path)
    {
        npc.isTalking = true;
        //dialogueText.text = Resources.Load<Text>("Talking_Text/" + path).text;
        dialogueText.text = "Oggi è bloccato";
        yield return new WaitForSeconds(audioSource.clip.length);
        npc.isTalking = false;
        dialogueText.text = "";
    }
}
