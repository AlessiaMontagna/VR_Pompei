using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MercatoFoodManager : MonoBehaviour
{
    [SerializeField] private int _fruitCounter;
    [SerializeField] private int _fishCounter;
    [SerializeField] private int _breadCounter;
    [SerializeField] private int _maxFruit = 6;
    [SerializeField] private int _maxBread = 4;
    [SerializeField] private int _maxFish = 3;

    [SerializeField] private TextMeshProUGUI _nFruitToTake;
    [SerializeField] private TextMeshProUGUI _nBreadToTake;
    [SerializeField] private TextMeshProUGUI _nFishToTake;

    [SerializeField] private List<Sprite> _imagesFinish;
    [SerializeField] private Image _FishFinish;
    [SerializeField] private Image _FruitFinish;
    [SerializeField] private Image _BreadFinish;

    [SerializeField] private GameObject _pergamena;


    private void Start()
    {
        _nFruitToTake.text = _maxFruit.ToString();
        _nBreadToTake.text = _maxBread.ToString();
        _nFishToTake.text = _maxFish.ToString();
    }
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
                return _maxBread;
            case foodType.Pesce:
                return _maxFish;
        }
        return -1;
    }
    public void addCounter(foodType _foodtype)
    {
        switch (_foodtype)
        {
            case foodType.Frutta:
                _fruitCounter++;
                _nFruitToTake.text = (_maxFruit - _fruitCounter).ToString();
                break;
            case foodType.Pesce:
                _fishCounter++;
                _nFishToTake.text = (_maxFish - _fishCounter).ToString();
                break;
            case foodType.Pane:
                _breadCounter++;
                _nBreadToTake.text = (_maxBread - _breadCounter).ToString();
                break;
        }
    }
    public void CheckMissionComplete()
    {
        if (_fruitCounter == _maxFruit && _maxBread == _breadCounter && _fishCounter == _maxFish)
        {
            _pergamena.SetActive(false);
        }
    }

    public void CheckGetMaxFood(foodType _foodtype)
    {
        switch (_foodtype)
        {
            case foodType.Frutta:
                if(_fruitCounter == _maxFruit)
                {
                    _FruitFinish.sprite = _imagesFinish[0];
                }
                break;
            case foodType.Pane:
                if (_breadCounter == _maxBread)
                {
                    _BreadFinish.sprite = _imagesFinish[1];
                }
                break;
            case foodType.Pesce:
                if (_fishCounter == _maxFish)
                {
                    _FishFinish.sprite = _imagesFinish[2];

                }
                break;
        }
    }
}
