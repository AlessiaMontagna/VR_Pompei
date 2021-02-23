using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject _UIKeyboard;
    [SerializeField] private GameObject _UIJoystick;

    private TextMeshProUGUI _dateText;

    private void Awake()
    {
        if(Globals.input == Inputs.Joystick.ToString())
        {
            _UIJoystick.SetActive(true);
            _UIKeyboard.SetActive(false);
        }
        else
        {
            _UIJoystick.SetActive(false);
            _UIKeyboard.SetActive(true);
        }
        _dateText = FindObjectOfType<DataInizio>().GetComponent<TextMeshProUGUI>();
        StartCoroutine(Date());
    }

    private IEnumerator Date()
    {
        if(Globals.language == "it")
        {
            _dateText.text = "24 Ottobre, 79 d.C. Ore 10:00";
        }
        else
        {
            _dateText.text = "24 October, 79 AD. 10 AM";

        }

        yield return new WaitForSeconds(5);
        _dateText.text = "";

    }


}
