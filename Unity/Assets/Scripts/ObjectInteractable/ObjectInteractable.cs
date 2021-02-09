using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInteractable : MonoBehaviour
{
    public abstract void UITextOn();
    public abstract void UITextOff();
    public abstract void Interact();
}
