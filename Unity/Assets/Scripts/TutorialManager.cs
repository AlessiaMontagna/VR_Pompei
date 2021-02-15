using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private BoxCollider _collider;

    private TextMeshProUGUI _tutorialText;
    private NpcTutorial _schiavo;
    private TextMeshProUGUI _obiettiviText;
    private ShowAgenda _scriptAgenda;
    private float _waitTime = 1f;
    private int _missionIndex;

    private void Start()
    {
        _tutorialText = FindObjectOfType<TutorialText>().GetComponent<TextMeshProUGUI>();
        _schiavo = FindObjectOfType<NpcTutorial>();
        _scriptAgenda = FindObjectOfType<ShowAgenda>();
        _scriptAgenda.enabled = false;
        _missionIndex = 0;
        _obiettiviText = FindObjectOfType<ObiettiviText>().GetComponent<TextMeshProUGUI>();
        _obiettiviText.text = "";
        _tutorialText.text = "Muoviti nell'ambiente usando il mouse e cammina usando i tasti WASD";
    }
    private void Update()
    {
        if(_schiavo == null) _schiavo = FindObjectOfType<NpcTutorial>();
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
                PutDownMappa();
                break;
            case 5:
                CheckCodex();
                break;
            case 6:
                FinishTutorial();
                break;
            default: throw new System.ArgumentOutOfRangeException();
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
        if (_waitTime <= 0)
        {
            _tutorialText.text = "Interagisci con oggetti e personaggi premendo il tasto indicato";
            _missionIndex = 2;
        }
        else
        {
            _waitTime -= Time.deltaTime;
        }
        
    }

    private void CheckInteraction()
    {
        if (_schiavo.pointingSchiavo && !_schiavo.hasTalked)
        {
            _tutorialText.text = "";
        }
        if(_schiavo.hasTalked)
        {
            _obiettiviText.text = "Nuovo obiettivo: raggiungi Marcus al foro";
            _tutorialText.text = "Premi  <sprite index= 0> per visualizzare la mappa";
            _scriptAgenda.enabled = true;
            _missionIndex = 3;
        }
    }
    
    private void CheckMappa()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _collider.enabled = false;
            _tutorialText.text = "Quì vengono mostrati i punti di interesse per i vari obiettivi. Premi nuovamente <sprite index= 0> per chiudere la mappa";
            _missionIndex = 4;
        }
    }
    private void PutDownMappa()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _tutorialText.enabled = false;
            this.enabled = false;
        }
        
    }

    private void CheckCodex()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _tutorialText.text = "Alcuni monumenti presenti nell'ambiente contengono informazioni storiche. Quando ci passi vicino, queste informazioni verranno salvate in questa sezione.";
            _missionIndex = 6;
        }
    }

    private void FinishTutorial()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _tutorialText.enabled = false;
            this.enabled = false;
        }
    }
}
