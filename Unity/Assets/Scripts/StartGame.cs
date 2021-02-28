using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject _UIKeyboard;
    [SerializeField] private GameObject _UIJoystick;

    [SerializeField] private GameObject _UIPauseIt;
    [SerializeField] private GameObject _UIPauseEng;

    private TextMeshProUGUI _dateText;
    private TextMeshProUGUI _obiettiviText;


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

        if(Globals.language == "it")
        {
            _UIPauseEng.SetActive(false);
        }
        else
        {
            _UIPauseIt.SetActive(false);
        }
        _dateText = FindObjectOfType<DataInizio>().GetComponent<TextMeshProUGUI>();
        _obiettiviText = FindObjectOfType<ObiettiviText>().GetComponent<TextMeshProUGUI>();
        StartCoroutine(Date());
    }

    private IEnumerator Date()
    {
        if(Globals.language == "it")
        {

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                _dateText.text = "23 Ottobre, 79 d.C. Ore 10:00";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                _dateText.text = "24 Ottobre, 79 d.C. Ore 20:00";
                _obiettiviText.text = "Nuovo obiettivo: Fuggi";
            }
            else if(SceneManager.GetActiveScene().buildIndex == 3)
            {
                _dateText.text = "24 Ottobre, 79 d.C. Ore 22:00";
                _obiettiviText.text = "Nuovo obiettivo: Trova una via di fuga";
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                _dateText.text = "79 AD, October 23th. 10 AM";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                _dateText.text = "79 AD, October 24th. 8 PM";
                _obiettiviText.text = "New Goal: run away";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                _dateText.text = "79 AD, October 24th. 10 PM";
                _obiettiviText.text = "New Goal: find a getaway";

            }

        }

        yield return new WaitForSeconds(3.27f);
        _dateText.text = "";

    }


}
