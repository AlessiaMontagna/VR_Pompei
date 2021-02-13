using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMercante : NpcInteractable
{
    [SerializeField] private MercanteFoodTypes _foodType;
    private Transform food;
    private bool foodUnloked = false;

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
            default: throw new System.ArgumentOutOfRangeException();
        }
    }

    private void EnableFood()
    {
        if (!foodUnloked && Globals.player == Players.Schiavo)
        {
            foreach (Transform child in food.transform)child.GetComponent<BoxCollider>().enabled = true;
            foodUnloked = true;
        }
    }

    protected override IEnumerator StartInteraction()
    {
        EnableFood();
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
