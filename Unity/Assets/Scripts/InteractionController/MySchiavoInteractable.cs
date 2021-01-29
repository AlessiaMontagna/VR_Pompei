using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySchiavoInteractable : Interattivo
{
    private Schiavo character;
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;
    [SerializeField] private Amico Mission1;
    private bool _switch = false;
    public bool _isTalking = false;
    void Start()
    {
        Mission1.Mission1Complete += Mission_1_unlocked;
        talk = FindObjectOfType<talk>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        character = gameObject.GetComponent<Schiavo>();
    }
    public override void Interact(GameObject caller)
    {
        character.Talk(_switch, caller);
        
    }
    public override void UITextOn()
    {
        if (!_isTalking)
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

    private void Mission_1_unlocked()
    {
        _switch = true;
    }
}
