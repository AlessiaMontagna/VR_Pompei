using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMercante : NpcInteractable
{
    [SerializeField] private MercanteFoodTypes _foodType;
    private Transform food;
    private bool unlockFood = true;

    void Start()
    {
        switch (_foodType)
        {
            case MercanteFoodTypes.Frutta:
                food = FindObjectOfType<Frutta>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Pane:
                food = FindObjectOfType<Pane>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Pesce:
                food = FindObjectOfType<Pesce>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Vasi:
                food = FindObjectOfType<Vaso>().GetComponent<Transform>();
                break;
            case MercanteFoodTypes.Verdura:
                food = FindObjectOfType<Verdura>().GetComponent<Transform>();
                break;
        }
    }

    private void EnableFood()
    {
        if (unlockFood && Globals.player == Players.Schiavo)
        {
            foreach (Transform child in food.transform)child.GetComponent<BoxCollider>().enabled = true;
            unlockFood = false;
        }
    }

    protected override IEnumerator StartInteraction()
    {
        return base.StartInteraction();
    }

    protected override void UpdateInteraction()
    {
        base.UpdateInteraction();
    }

    protected override void StopInteraction()
    {
        base.StopInteraction();
    }
}
