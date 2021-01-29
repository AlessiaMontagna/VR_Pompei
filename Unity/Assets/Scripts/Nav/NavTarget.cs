using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTarget : MonoBehaviour
{
    [SerializeField] private string _role;
    // Start is called before the first frame update
    void Start(){}
    public string GetRole(){return _role;}
}