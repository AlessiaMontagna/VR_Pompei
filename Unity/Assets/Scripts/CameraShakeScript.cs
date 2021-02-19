using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{    
    public AudioSource audioSource;
    bool AlreadyPlayed = false;
    bool stop = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();        
        audioSource.Stop();        
    }

    public IEnumerator Shake (float duration, float magnitude)  //public -> can access this script from external scripts
    {
        //play
        if(!AlreadyPlayed)
        {
            audioSource.Play();
            audioSource.volume = 0;
            AlreadyPlayed = true;
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
            if(audioSource.volume <= 1f && !stop) audioSource.volume = audioSource.volume + delta;
            if(audioSource.volume >= 0 && stop)
            {
                audioSource.volume = audioSource.volume - delta;
            }
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x,y, originalPos.z);

            time_elapsed += Time.deltaTime; //increase elapsed time by the time that has gone by
            
            yield return null; // run this coroutine every time Update() is called, does 1 iteration of while per frame
        }
        //stop
        audioSource.Stop();
        AlreadyPlayed = false;
        transform.localPosition = originalPos; //restore original position
    }
}
