using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTarget : MonoBehaviour
{
    [SerializeField] private string _type;
    // Start is called before the first frame update
    void Start(){}
    public string GetType(){return _type;}
}