using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interattivo : MonoBehaviour
{
    public abstract void Interact(GameObject caller);
    public abstract void UITextOn();
    public abstract void UITextOff();
}

