using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercante : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject mele;
    System.Random rnd;
    private int index;
    private string[] voci = { "Antonio", "Francesco", "Giorgio" };
    private int i;
    private string voce;
    private bool unlockMele = true;
    private void Start()
    {
        rnd = new System.Random();
        i = rnd.Next(1, 3);
        voce = voci[i];
    }
    public void Talk(GameObject caller)
    {
        
        index = rnd.Next(1, 4);
        if (!audioSource.isPlaying)
        {
            switch (caller.tag)
            {
                case "Player_Nobile":
                    audioSource.clip = Resources.Load<AudioClip>("Talking/Nobile_Mercante" + index.ToString() + voce);
                    audioSource.Play();
                    break;
                case "Player_Schiavo":
                    audioSource.clip = Resources.Load<AudioClip>("Talking/Schiavo_Mercante1" + voce);
                    if (unlockMele)
                    {
                        for (int i = 0; i < mele.transform.childCount; i++)
                        {
                            mele.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                        }
                        unlockMele = false;
                    }
                    audioSource.Play();
                    break;
            }
        }
    }
}
