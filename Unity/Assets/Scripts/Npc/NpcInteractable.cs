using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcInteractable : Interattivo
{

    private NpcSuperClass _character;
    [SerializeField] private Text _talk;
    [SerializeField] private RawImage _eButton;
    
    void Start()
    {
        _talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        _character = GetComponent<NpcSuperClass>();
    }

    public override void Interact(GameObject caller)
    {
        _character.Interact();
    }

    public override void UITextOn()
    {
        _eButton.enabled = true;
        _talk.enabled = true;
    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        _talk.enabled = false;
    }
}
