using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{    
    AudioSource audio;
    bool AlreadyPlayed = false;
    bool stop = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();        
        audio.Stop();        
    }

    public IEnumerator Shake (float duration, float magnitude)  //public -> can access this script from external scripts
    {
        //play
        if(!AlreadyPlayed)
        {
            audio.Play();
            audio.volume = 0;
            AlreadyPlayed = true;
        }

        float threshold = duration*0.87f;
                
        Vector3 originalPos = transform.localPosition;
        
        float time_elapsed = 0.0f;        
        
        while (time_elapsed < duration)
        {
            if(time_elapsed >= threshold) stop = true;
            //fade in
            if(audio.volume <= 1f && !stop) audio.volume = audio.volume + 0.05f;
            if(audio.volume >= 0 && stop)
            {
                audio.volume = audio.volume - 0.05f;
            }
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x,y, originalPos.z);

            time_elapsed += Time.deltaTime; //increase elapsed time by the time that has gone by
            
            yield return null; // run this coroutine every time Update() is called, does 1 iteration of while per frame
        }
        //stop
        audio.Stop();
        AlreadyPlayed = false;
        transform.localPosition = originalPos; //restore original position
    }
}
