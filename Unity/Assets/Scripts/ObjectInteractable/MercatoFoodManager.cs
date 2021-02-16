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

    private CameraShakeScript _cameraShake;

    private void Awake()
    {
        _cameraShake = FindObjectOfType<CameraShakeScript>();
        _pergamena = FindObjectOfType<Pergamena>().gameObject;

    }
    private void Start()
    {
        if(_imagesFinish.Count != 3)
        {
            Debug.LogError("Inserisci le immagini contenute in Textures/UI/Cibo. pos 0 = frutta, pos 1 = pane, pos 2 = pesce");
        }
        _FruitFinish = _pergamena.transform.Find("Frutta").GetComponent<Image>();
        _BreadFinish = _pergamena.transform.Find("Pane").GetComponent<Image>();
        _FishFinish = _pergamena.transform.Find("Pesce").GetComponent<Image>();

        _nFruitToTake = _pergamena.transform.Find("NFrutta").GetComponent<TextMeshProUGUI>();
        _nBreadToTake = _pergamena.transform.Find("NPane").GetComponent<TextMeshProUGUI>();
        _nFishToTake = _pergamena.transform.Find("NPesce").GetComponent<TextMeshProUGUI>();


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
            StartCoroutine(Wait());

        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(_cameraShake.Shake(10f, .2f));
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
