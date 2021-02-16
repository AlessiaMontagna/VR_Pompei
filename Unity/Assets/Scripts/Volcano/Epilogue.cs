using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epilogue : MonoBehaviour
{
    public GameObject Player;
    public GameObject SmokeColumn;
    public GameObject Barrier;
    public LevelChangerScript level;
    bool _first = false;

    void Start() 
    {
        SmokeColumn.SetActive(false);
    }
   
    void OnTriggerEnter(Collider other) 
    {        
        if(other.gameObject == Player && !_first)
        {
            Debug.Log("Player entered the Trigger: Epiogue");
            _first = true;
            Barrier.SetActive(true);               
            StartCoroutine("Smoke");
        }
        else
            return;        
    }

    public IEnumerator Smoke()
    {
        yield return new WaitForSeconds(10.0f);  
        SmokeColumn.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        level.FadeToLevel(0); //torna al menu
    }
}
