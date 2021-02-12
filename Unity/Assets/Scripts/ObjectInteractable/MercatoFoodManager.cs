using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercatoFoodManager : MonoBehaviour
{
    private int _fruitCounter = 0;
    private int _fishCounter = 0;
    private int _breadCounter = 0;
    [SerializeField] private int _maxFruit = 5;
    [SerializeField] private int _maxBread = 2;
    [SerializeField] private int _maxFish = 3;

    public int getCounter(foodType _foodtype)
    {
        switch (_foodtype)
        {
            case foodType.Frutta:
                return _fruitCounter;
            case foodType.Pesce:
                return _fishCounter;
            case foodType.Pane:
                return _breadCounter;
        }
        return -1;
    }

    public int getMax(foodType _foodtype)
    {
        switch (_foodtype)
        {
            case foodType.Frutta:
                return _maxFruit;
            case foodType.Pane:
                return _maxFish;
            case foodType.Pesce:
                return _maxBread;
        }
        return -1;
    }
    public void addCounter(foodType _foodtype)
    {
        switch (_foodtype)
        {
            case foodType.Frutta:
                _fruitCounter++;
                break;
            case foodType.Pesce:
                _fishCounter++;
                break;
            case foodType.Pane:
                _breadCounter++;
                break;
        }
    }
    public void CheckMissionComplete()
    {
        if (_fruitCounter == _maxFruit && _maxBread == _breadCounter && _fishCounter == _maxFish)
        {
            Debug.Log("Missione completata");
        }
    }

}
