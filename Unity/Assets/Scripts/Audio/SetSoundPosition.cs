using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSoundPosition : MonoBehaviour
{   
    public Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
