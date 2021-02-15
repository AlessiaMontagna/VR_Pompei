using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NpcMercante : NpcInteractable
{
    private Transform _food;
    private bool _foodUnloked = false;

    void Start()
    {
        if(navAgent == null) Initialize(Characters.Mercante, FindObjectOfType<NavSpawner>().gameObject, "Idle", null);
        else switch(gameObject.GetComponentInParent<NavElement>().foodType)
        {
            case MercanteFoodTypes.Frutta: _food = FindObjectOfType<Frutta>().GetComponent<Transform>();break;
            case MercanteFoodTypes.Pane: _food = FindObjectOfType<Pane>().GetComponent<Transform>();break;
            case MercanteFoodTypes.Pesce: _food = FindObjectOfType<Pesce>().GetComponent<Transform>();break;
            case MercanteFoodTypes.Vasi: _food = FindObjectOfType<Vaso>().GetComponent<Transform>();break;
            case MercanteFoodTypes.Verdura: _food = FindObjectOfType<Verdura>().GetComponent<Transform>();break;
            default: throw new System.ArgumentOutOfRangeException();
        }
    }

    protected override void StartInteraction()
    {
        if (!_foodUnloked && Globals.player == Players.Schiavo)
        {
            foreach (Transform child in _food.transform) child.GetComponent<BoxCollider>().enabled = true;
            _foodUnloked = true;
        }
        base.StartInteraction();
    }
}
