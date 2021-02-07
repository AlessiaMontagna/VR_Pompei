using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private Text _tutorialText;
    [SerializeField] private Text _obiettiviText;
    [SerializeField] private SchiavoInteractable _schiavo;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private ShowAgenda _scriptAgenda;
    [SerializeField] private SwitchWhatIsShowing _mco_text;
    private float _waitTime = 5f;
    private int _missionIndex;
    [SerializeField] private Text _agendaText;

    private void Start()
    {
        _scriptAgenda.enabled = false;
        _missionIndex = 0;
        _obiettiviText.text = "";
        _agendaText.text = "";
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
        if(_schiavo.index == 2)
        {
            _tutorialText.text = "Buongiorno Signore. Il mio padrone la sta aspettando";
            if(_waitTime <= 0)
            {
                _obiettiviText.text = "Nuovo obiettivo: Raggiungi i tuoi amici al foro";
                _tutorialText.text = "Premi TAB per aprire la mappa";
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _tutorialText.text = "Quì puoi visualizzare mappa, codex e obiettivi da completare. Utilizza i pulsanti mostrati per muoverti tra le varie componenti, premi nuovamente tab per chiudere";
            _missionIndex = 4;
        }
    }
    private void MCOText()
    {
        if (_mco_text.indexAgenda == (byte)0)
        {
            _agendaText.text = "Il simbolo della freccia rappresenta la tua posizione, il simbolo rosso lampeggiante rappresenta la direzione verso cui devi andare";
        }else if(_mco_text.indexAgenda == (byte)1)
        {
            _agendaText.text = "IN questa sezione potrai visualizzare le informazioni storiche raccolte durante l'esperienza di gioco. ";
        }else if (_mco_text.indexAgenda == (byte)2)
        {
            _agendaText.text = "Quì potrai visualizzare la tua lista di obiettivi da completare";
        }
        if (CrossPlatformInputManager.GetButtonDown("Agenda"))
        {
            _agendaText.text = "";
            _missionIndex = 5;
        }
    }
    private void FinishTutorial()
    {
        
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
