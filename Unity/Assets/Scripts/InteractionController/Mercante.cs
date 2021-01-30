using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mercante : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject food;
    [SerializeField] private Text _dialogueText;
    [SerializeField] private string _foodType;
    //BancarellaTrigger b;
    System.Random rnd;
    private int index;
    private bool unlockFood = true;
    private string path;
    
    private void Start()
    {
        rnd = new System.Random();
    }
    public void Talk(GameObject caller)
    {
        index = rnd.Next(1, 4);
        if (!audioSource.isPlaying)
        {
            switch (caller.tag)
            {
                case "Player_Nobile":
                    path = "Nobile_Mercante" + index + "_" + _foodType;
                    audioSource.clip = Resources.Load<AudioClip>("Talking/" + path);
                    audioSource.Play();
                    break;
                case "Player_Schiavo":
                    path = "Schiavo_Mercante" + "_" + _foodType;
                    audioSource.clip = Resources.Load<AudioClip>("Talking/" + path);
                   
                    //b.index = 1;
                    if (unlockFood)
                    {
                        for (int i = 0; i < food.transform.childCount; i++)
                        {
                            food.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                        }
                        unlockFood = false;
                    }
                    audioSource.Play();
                    break;
            }

            StartCoroutine(StartDialogue(audioSource.clip, path));
        }
    }

    private IEnumerator StartDialogue(AudioClip clip, string path)
    {
        //_dialogueText.text = Resources.Load<Text>("Talking_Text/" + path).text;
        //yield return new WaitForSeconds(clip.length);
        _dialogueText.text = "Ottima frutta oggi";
        yield return new WaitForSeconds(2);
        _dialogueText.text = "";
    }
}
