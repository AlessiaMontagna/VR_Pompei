﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _tutorialText;
    [SerializeField] private SchiavoInteractable _schiavo;
    [SerializeField] private BoxCollider _collider;
    //[SerializeField] private Text _agendaText;

    private TextMeshProUGUI _obiettiviText;
    private ShowAgenda _scriptAgenda;
    private float _waitTime = 5f;
    private int _missionIndex;

    private void Start()
    {
        _scriptAgenda = FindObjectOfType<ShowAgenda>();
        _scriptAgenda.enabled = false;
        _missionIndex = 0;
        _obiettiviText = FindObjectOfType<ObiettiviText>().GetComponent<TextMeshProUGUI>();
        _obiettiviText.text = "";
       // _agendaText.text = "";
        _tutorialText.text = "Muoviti nell'ambiente usando il mouse e cammina usando i tasti WASD";
    }
    private void Update()
    {
        switch(_missionIndex)
        {
            case 0:
                CheckWalk();
                break;
            case 1:
                CheckPointing();
                break;
            case 2:
                CheckInteraction();
                break;
            case 3:
                CheckMappa();
                break;
            case 4:
                MCOText();
                break;
            case 5:
                FinishTutorial();
                break;
        }
    }

    private void CheckWalk()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            _missionIndex = 1;
            _tutorialText.text = "";
        }
        
    }

    private void CheckPointing()
    {
        if (_schiavo.pointingSchiavo)
        {
            _tutorialText.text = "Interagisci con oggetti e personaggi premendo il tasto indicato";
            _missionIndex = 2;
        }
    }

    private void CheckInteraction()
    {
        if(_schiavo.hasTalked)
        {
            _tutorialText.text = "";
            if(_waitTime <= 0)
            {
                _obiettiviText.text = "Nuovo obiettivo: raggiungi Marcus al foro";
                _tutorialText.text = "Premi () per visualizzare la mappa";
                _scriptAgenda.enabled = true;
                _missionIndex = 3;
                _waitTime = 5f;
            }
            else
            {
                _waitTime -= Time.deltaTime;
            }
        }
    }
    
    private void CheckMappa()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _tutorialText.text = "Quì potrai visualizzare la tua posizione e il punto di destinazione. Premi nuovamente () per riporre la mappa.";
            _missionIndex = 4;
        }
    }
    private void MCOText()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _tutorialText.text = "";
            _missionIndex = 5;
        }
    }
    private void FinishTutorial()
    {
        //TODO: lo schiavo se ne deve andare
        _tutorialText.text = "Adesso sai tutto. Completa gli obiettivi che ti vengono mostrati in alto a destra. Buona fortuna!";
        _collider.enabled = false;
        if (_waitTime <= 0)
        {
            _tutorialText.enabled = false;
            this.enabled = false;
        }
        else
        {
            _waitTime -= Time.deltaTime;
        }

        
    }
}
