using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{    
    public AudioSource _audio;
    bool AlreadyPlayed = false;
    bool stop = false;

    void Start()
    {
        //audio = GetComponent<AudioSource>();        
        GetComponent<AudioSource>().Stop();        
    }

    public IEnumerator Shake (float duration, float magnitude)  //public -> can access this script from external scripts
    {
        //play
        if(!AlreadyPlayed)
        {
            _audio.Play();
            _audio.volume = 0;
            AlreadyPlayed = true;
        }        
                
        Vector3 originalPos = transform.localPosition;
        
        float time_elapsed = 0.0f;        
        
        while (time_elapsed < duration)
        {
            float frames = 1.0f/Time.deltaTime;
            float threshold = duration/2;
            float delta = 1.0f/(frames*5.0f);//frames/(duration - threshold);

            Debug.Log("Frames: " + frames);
            Debug.Log("threshold: " + threshold);
            Debug.Log("delta: " + delta);
            Debug.Log("duration: " + duration);

            if(time_elapsed >= threshold) stop = true;
            //fade in
            if(_audio.volume <= 1f && !stop) _audio.volume = _audio.volume + 0.05f;
            if(_audio.volume >= 0 && stop)
            {
                _audio.volume = _audio.volume - 0.05f;
            }
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x,y, originalPos.z);

            time_elapsed += Time.deltaTime; //increase elapsed time by the time that has gone by
            
            yield return null; // run this coroutine every time Update() is called, does 1 iteration of while per frame
        }
        //stop
        _audio.Stop();
        AlreadyPlayed = false;
        transform.localPosition = originalPos; //restore original position
    }
}
