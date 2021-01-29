using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodInteractive : Interattivo
{
    [SerializeField] private Food food;
    [SerializeField] private Text pick;
    [SerializeField] private Color highlightColor;
    [SerializeField] private RawImage _eButton;
    private void Start()
    {
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        food = GetComponent<Food>();
        highlightColor = new Color(0.3962264f, 0.3962264f, 0.3962264f, 1);
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    }
    
    public override void Interact(GameObject caller)
    {
        food.getFood();
    }

    public override void UITextOn()
    {
        _eButton.enabled = true;
        pick.enabled = true;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", highlightColor);
    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        pick.enabled = false;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    }
}
