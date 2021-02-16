using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    [SerializeField] private Camera _myCamera;

    void Update()
    {
        transform.LookAt(_myCamera.transform);
    }
}
