using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour
{    
    public IEnumerator Shake (float duration, float magnitude)  //public -> can access this script from external scripts
    {
        Vector3 originalPos = transform.localPosition;

        float time_elapsed = 0.0f;

        while (time_elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x,y, originalPos.z);

            time_elapsed += Time.deltaTime; //increase elapsed time by the time that has gone by
            
            yield return null; // run this coroutine every time Update() is called, does 1 iteration of while per frame
        }

        transform.localPosition = originalPos; //restore original position
    }
}
