using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationObject : ObjectInteractable
{
    [SerializeField] private CodexInformation _objectName;
    [SerializeField] private Text _nuovaVoceText;


    public override void Interact()
    {
    }

    public override void UITextOff()
    {
    }

    public override void UITextOn()
    {
        StartCoroutine(NewVoice());
        foreach(BoxCollider b in GetComponents<BoxCollider>())
        {
            b.enabled = false;
        }
    }

    private IEnumerator NewVoice()
    {
        _nuovaVoceText.text = "Nuova nota nel codex: " + _objectName.ToString();
        yield return new WaitForSeconds(5);
        if(_nuovaVoceText.text == "Nuova nota nel codex: " + _objectName.ToString()) _nuovaVoceText.text = "";

    }
}
