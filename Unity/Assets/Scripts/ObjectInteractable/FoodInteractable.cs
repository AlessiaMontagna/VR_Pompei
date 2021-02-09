using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum foodType { Frutta, Pesce, Pane, NonInteractable };
public class FoodInteractable : ObjectInteractable
{
    [SerializeField] private foodType _foodType;
    [SerializeField] private Text pick;
    [SerializeField] private RawImage _eButton;

    private Color highlightColor;
    private MercatoFoodManager _foodManager;
    private Text dialogueText;


    private void Start()
    {
        _foodManager = FindObjectOfType<MercatoFoodManager>();
        pick = FindObjectOfType<Pick>().GetComponent<Text>();
        dialogueText = FindObjectOfType<sottotitoli>().GetComponent<Text>();
        _eButton = FindObjectOfType<eButton>().GetComponent<RawImage>();
        highlightColor = new Color(0.3962264f, 0.3962264f, 0.3962264f, 1);
        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    }

    public override void Interact()
    {
        if(_foodType == foodType.NonInteractable)
        {
            StartCoroutine(startDialogue());
            return;
        }
        if (_foodManager.getCounter(_foodType) < _foodManager.getMax(_foodType))
        {
            UITextOff();
            _foodManager.addCounter(_foodType);
            _foodManager.CheckMissionComplete();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(startDialogue());
            //TODO: Play audio tizio
        }

    }

    public override void UITextOff()
    {
        _eButton.enabled = false;
        pick.enabled = false;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
    }

    public override void UITextOn()
    {
        _eButton.enabled = true;
        pick.enabled = true;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", highlightColor);
    }

    private IEnumerator startDialogue()
    {
        dialogueText.text = "Non posso prendere più cose";
        yield return new WaitForSeconds(2);
        if (dialogueText.text == "Non posso prendere più cose")
            dialogueText.text = "";
    }
}
