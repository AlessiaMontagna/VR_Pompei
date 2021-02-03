using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Guardia))]
public class GuardiaInteractable : Interattivo
{
    private Guardia character;
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;

    void Start()
    {
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        character = gameObject.GetComponent<Guardia>();
    }
    public override void Interact(GameObject caller)
    {
        character.Talk(caller);
    }


    public override void UITextOn()
    {
        talk.enabled = true;
        _eButton.enabled = true;
    }

    public override void UITextOff()
    {
        talk.enabled = false;
        _eButton.enabled = false;
    }
}
