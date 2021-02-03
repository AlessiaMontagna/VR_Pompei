using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcInteractable : Interattivo
{

    private Npc character;
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;
    void Start()
    {
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        character = GetComponent<Npc>();
    }

    public override void Interact(GameObject caller)
    {
        character.Interact();
    }

    public override void UITextOn()
    {
        _eButton.enabled = true;
        talk.enabled = true;
    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        talk.enabled = false;
    }
}
