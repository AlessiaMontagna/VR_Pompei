using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    [SerializeField] private string foodType;
    [SerializeField] private FoodManager foodmanager;
    [SerializeField] private Text dialogueText;
    public void getFood()
    {
        if(foodmanager.getCounter(foodType) < foodmanager.getMax(foodType))
        {
            Destroy(gameObject);
            foodmanager.addCounter(foodType);
            foodmanager.CheckMissionComplete();
        }
        else
        {
            StartCoroutine(startDialogue());
            //TODO: Play audio tizio
        }
    }

    private IEnumerator startDialogue()
    {
        dialogueText.text = "Non posso prendere più cose";
        yield return new WaitForSeconds(2);
        dialogueText.text = "";
    }
}
