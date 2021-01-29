using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private Color highlightColor;
    [SerializeField] private Transform mele;
    void Start()
    {
        highlightColor = new Color(0.3962264f, 0.3962264f, 0.3962264f, 1);
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        
    }
    // Start is called before the first frame update
    public void getFood()
    {
        Destroy(gameObject);
        for( int i = 0; i < mele.childCount; i++)
        {
            mele.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        }
        
    }
}
