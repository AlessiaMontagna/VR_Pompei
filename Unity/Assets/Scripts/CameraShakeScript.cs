using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{    
    public GameObject[] Tiles;
    public GameObject[] Particles;
    private AudioSource _audioSource;    
    private bool AlreadyPlayed = false;
    private bool stop = false;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();        
        _audioSource.Stop();
        if(Tiles.Length == 0) return;  //WARNING!
            else
            {
                for(int i=0; i<Tiles.Length; i++)
                {
                    Tiles[i].SetActive(false);
                }
            }                       
    }

    public IEnumerator Shake (float duration, float magnitude)  //public -> can access this script from external scripts
    {
       
        //play
        if (!AlreadyPlayed)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "ScenaFinale") Globals.earthquake = true;
            _audioSource.Play();
            _audioSource.volume = 0;
            AlreadyPlayed = true;
            
            if(Tiles.Length == 0 || Particles.Length == 0) yield return null;
            else
            {
                for(int i=0; i<Tiles.Length; i++)
                {
                    Tiles[i].SetActive(true);
                }
                for(int i=0; i<Particles.Length; i++)   //attivo i particles di sgretolamento da terremoto
                {                    
                    ParticleSystem ps = Particles[i].GetComponent<ParticleSystem>();
                    ps.Stop();
                    var main = ps.main;
                    main.duration = duration - 4.0f;                                   
                    ps.Play();
                }
            }
        }
                
        Vector3 originalPos = transform.localPosition;
        
        float time_elapsed = 0.0f;        
        
        while (time_elapsed < duration)
        {
            float frames = 1.0f/Time.deltaTime;
            float threshold = duration/2;
            float delta = 1.0f/(frames*5.0f);//frames/(duration - threshold);

            if(time_elapsed >= threshold) stop = true;
            //fade in
            if(_audioSource.volume <= 1f && !stop) _audioSource.volume = _audioSource.volume + delta;
            //fade out
            if(_audioSource.volume >= 0 && stop)
            {
                _audioSource.volume = _audioSource.volume - delta;
            }
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x,y, originalPos.z);

            time_elapsed += Time.deltaTime; //increase elapsed time by the time that has gone by
            
            yield return null; // run this coroutine every time Update() is called, does 1 iteration of while per frame
        }
        //stop
        _audioSource.Stop();
        if(Tiles.Length == 0) yield return null;
            else
            {
                for(int i=0; i<Tiles.Length; i++)
                {
                    Tiles[i].GetComponent<AudioSource>().Stop();
                    Tiles[i].SetActive(false);
                }
            }
        AlreadyPlayed = false;
        transform.localPosition = originalPos; //restore original position
    }
}
