﻿using System.Collections;
using System.Collections.Generic;
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

    public IEnumerator Schiavo()
    {   //todo:
        yield return new WaitForSeconds(1.0f);
            Debug.LogWarning("Manca audio Schiavo: 'Padrone, l'uscita principale è bloccata, deve cercare un'altra via di fuga!'");
    }
}