using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Explode1Script : MonoBehaviour
{
    public GameObject Player;
    public GameObject Debris;
    public GameObject Fire;
    public Transform ExplosionSpot;
    public CameraShakeScript cameraShake;    
    public float radius;
    public float power;
    public Epilogue explosion;
    bool _first = false;
    [SerializeField] AudioSource _audioSource;
    private TextMeshProUGUI _sub;

    void OnTriggerEnter(Collider other) 
    {        
        if(other.gameObject == Player && !_first)
        {
            Debug.Log("Player entered the Trigger: Explode1");
            _first = true;
            Vector3 explosionPos = ExplosionSpot.position;
            explosion.Explode(explosionPos, radius, power, Debris, Fire, cameraShake);
            StartCoroutine("Schiavo");
        }

    }

    private void Start()
    {
        _sub = FindObjectOfType<sottotitoli>().GetComponent<TextMeshProUGUI>();
    }
    public IEnumerator Schiavo()
    {   
        yield return new WaitForSeconds(2.57f);
        _audioSource.Play();
        if(_sub!= null)
        {
            if(Globals.language == "it")
            {
                _sub.text = "Padrone, l'uscita principale è bloccata, deve cercare un'altra via di fuga";
            }
            else
            {
                _sub.text = "Master, the main exit is blocked, you must look for another escape route";
            }
        }
        yield return new WaitForSeconds(_audioSource.clip.length);
        _sub.text = "";
    }
}
