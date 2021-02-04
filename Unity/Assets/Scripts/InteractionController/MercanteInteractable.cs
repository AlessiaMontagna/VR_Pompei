using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Mercante))]
[RequireComponent(typeof(AudioSource))]

public class MercanteInteractable : Interattivo
{
    private Mercante character;
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;
    public bool isTalking = false;


    void Start()
    {
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        character = gameObject.GetComponent<Mercante>();
    }
    public override void Interact(GameObject caller)
    {
        character.Talk(caller);
    }
    public override void UITextOn()
    {
        if (!isTalking)
        {
            talk.enabled = true;
            _eButton.enabled = true;
        }
    }

    public override void UITextOff()
    {
        talk.enabled = false;
        _eButton.enabled = false;
    }
}
