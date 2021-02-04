using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum FoodType {Frutta, Pane, Pesce};


public class Mercante : MonoBehaviour
{
    [SerializeField] private FoodType _foodType;

    private Transform food;
    private AudioSubManager _subtitles;
    private Text _dialogueText;
    private AudioSource audioSource;
    private bool unlockFood = true;
    private string path;
    private int index;
    private MercanteInteractable mercante;
    
    private void Start()
    {
        mercante = GetComponent<MercanteInteractable>();
        if(_foodType == FoodType.Frutta)
        {
            index = 0;
            Debug.Log(_foodType.ToString());
            food = FindObjectOfType<Frutta>().GetComponent<Transform>();

        }else if(_foodType == FoodType.Pane)
        {
            index = 1;
            food = FindObjectOfType<Pane>().GetComponent<Transform>();
            Debug.Log("indice pane" + index);
        }
        else if(_foodType == FoodType.Pesce)
        {
            food = FindObjectOfType<Pesce>().GetComponent<Transform>();
            index = 2;
            Debug.Log("indice pesce" + index);

        }
        
        _subtitles = FindObjectOfType<AudioSubManager>();
        _dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }
    public void Talk(GameObject caller)
    {
        if (!audioSource.isPlaying)
        {
            path = Globals.player.ToString() + "_Mercante" + "_" + _foodType.ToString();
            Debug.Log(path);
            audioSource.clip = Resources.Load<AudioClip>("Talking/" + path);
            if (audioSource.clip == null)
            {
                Debug.LogError("Non ci sono file audio nel path specificato");
                return;
            }
            audioSource.Play();
            if(unlockFood && Globals.player == Players.Schiavo)
            {
                for (int i = 0; i < food.childCount; i++)
                    food.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                unlockFood = false;
            }
            StartCoroutine(Sottotitoli());
        }
    }
    private IEnumerator Sottotitoli()
    {
        mercante.isTalking = true;
        _dialogueText.text = _subtitles.GetSubs(index, Characters.Mercante);
        Debug.Log(_dialogueText.text);
        yield return new WaitForSeconds(audioSource.clip.length);
        mercante.isTalking = false;
        _dialogueText.text = "";
    }

    
}
