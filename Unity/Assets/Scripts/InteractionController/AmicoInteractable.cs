﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmicoInteractable : Interattivo
{
    private Amico character;
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;
    public bool isTalking = false;

    void Start()
    {
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        character = gameObject.GetComponent<Amico>();
    }
    public override void Interact(GameObject caller)
    {
        character.Talk(caller);
    }
    public override void UITextOn()
    {
        if (!isTalking)
        {
            _eButton.enabled = true;
            talk.enabled = true;
        }
    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        talk.enabled = false;
    }
}
