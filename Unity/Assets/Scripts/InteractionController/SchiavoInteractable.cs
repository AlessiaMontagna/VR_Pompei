using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchiavoInteractable : Interattivo
{
    [SerializeField] private SchiavoTutorial character;
    [SerializeField] private Text talk;
    [SerializeField] private RawImage _eButton;
    public bool pointingSchiavo = false; 
    public bool hasTalked = false;
    public int index = 1;
    [SerializeField] private Animator _animator;


    public override void Interact(GameObject caller)
    {
        if(index == 1)
        {
            hasTalked = true;
            _eButton.enabled = false;
            talk.enabled = false;
            _animator.SetBool("Talk", true);
            character.Talk(Resources.Load<AudioClip>("Talking/Nobile_Schiavo" + index));
            index = 2;
        }
    }


    public override void UITextOn()
    {
        if (!hasTalked)
        {
            _eButton.enabled = true;
            talk.enabled = true;
            pointingSchiavo = true;
        }
    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        talk.enabled = false;
    }

}
