using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private int _fruitCounter = 0;
    private int _fishCounter = 0;
    private int _breadCounter = 0;
    [SerializeField] private int maxFruit = 5;
    [SerializeField] private int maxBread = 2;
    [SerializeField] private int maxFish = 3;


    public int getCounter(string foodtype)
    {
        switch (foodtype)
        {
            case "Frutta":
                return _fruitCounter;
            case "Pesce":
                return _fishCounter;
            case "Pane":
                return _breadCounter;
        }
        return -1;
    }
    public int getMax(string foodtype)
    {
        switch (foodtype)
        {
            case "Frutta":
                return maxFruit;
            case "Pesce":
                return maxFish;
            case "Pane":
                return maxBread;
        }
        return -1;
    }

    public void CheckMissionComplete()
    {
        if(_fruitCounter == maxFruit && maxBread == _breadCounter && _fishCounter == maxFish)
        {
            Debug.Log("Missione completata");
        }
    }
    public void addCounter(string foodtype)
    {
        switch (foodtype)
        {
            case "Frutta":
                _fruitCounter++;
                break;
            case "Pesce":
                _fishCounter++;
                break;
            case "Pane":
                _breadCounter++;
                break;
        }
    }
}
