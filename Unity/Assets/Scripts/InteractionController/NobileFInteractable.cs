using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NobileF))]
public class NobileFInteractable : Interattivo
{
    private NobileF character;
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;

    void Start()
    {
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        character = gameObject.GetComponent<NobileF>();
    }
    public override void Interact(GameObject caller)
    {
        character.Talk(caller);
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
