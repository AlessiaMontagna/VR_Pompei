using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudioScript : MonoBehaviour
{
    public ParticleSystem crater;
    public ParticleSystem lapillus;    

    public void StopParticles()
    {
        crater.Stop();        
        lapillus.Stop();
        Destroy(crater);
        Destroy(lapillus);
    }
}
