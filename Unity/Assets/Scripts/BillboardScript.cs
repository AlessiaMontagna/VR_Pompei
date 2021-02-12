using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    [SerializeField] private Camera _myCamera;

    void Update()
    {
        /*transform.LookAt(transform.position + _myCamera.transform.rotation * Vector3.back,
                        _myCamera.transform.rotation * Vector3.up);*/
        transform.LookAt(_myCamera.transform);
    }
}
