using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TutorialManager : MonoBehaviour
{

    [SerializeField] private Text tutorialText;
    [SerializeField] private Text obiettiviText;
    [SerializeField] private SchiavoInteractable schiavo;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private ShowAgenda scriptAgenda;
    [SerializeField] private SwitchWhatIsShowing mco_text;
    private float waitTime = 5f;
    private int missionIndex;
    [SerializeField] private Text agendaText;

    private void Start()
    {
        scriptAgenda.enabled = false;
        missionIndex = 0;
        obiettiviText.text = "";
        agendaText.text = "";
        tutorialText.text = "Muoviti nell'ambiente usando il mouse e cammina usando i tasti WASD";
    }
    private void Update()
    {
        switch(missionIndex)
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
            missionIndex = 1;
            tutorialText.text = "";
        }
        
    }

    private void CheckPointing()
    {
        if (schiavo.pointingSchiavo)
        {
            tutorialText.text = "Interagisci con oggetti e personaggi premendo il tasto indicato";
            missionIndex = 2;
        }
    }

    private void CheckInteraction()
    {
        if(schiavo.index == 2)
        {
            tutorialText.text = "Buongiorno Signore. Il mio padrone la sta aspettando";
            if(waitTime <= 0)
            {
                obiettiviText.text = "Nuovo obiettivo: Raggiungi i tuoi amici al foro";
                tutorialText.text = "Premi TAB per aprire la mappa";
                scriptAgenda.enabled = true;
                missionIndex = 3;
                waitTime = 5f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    
    private void CheckMappa()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tutorialText.text = "Quì puoi visualizzare mappa, codex e obiettivi da completare. Utilizza i pulsanti mostrati per muoverti tra le varie componenti, premi nuovamente tab per chiudere";
            missionIndex = 4;
        }
    }
    private void MCOText()
    {
        if (mco_text.indexAgenda == (byte)0)
        {
            agendaText.text = "Il simbolo della freccia rappresenta la tua posizione, il simbolo rosso lampeggiante rappresenta la direzione verso cui devi andare";
        }else if(mco_text.indexAgenda == (byte)1)
        {
            agendaText.text = "IN questa sezione potrai visualizzare le informazioni storiche raccolte durante l'esperienza di gioco. ";
        }else if (mco_text.indexAgenda == (byte)2)
        {
            agendaText.text = "Quì potrai visualizzare la tua lista di obiettivi da completare";
        }
        if (CrossPlatformInputManager.GetButtonDown("Agenda"))
        {
            agendaText.text = "";
            missionIndex = 5;
        }
    }
    private void FinishTutorial()
    {
        
        tutorialText.text = "Adesso sai tutto. Completa gli obiettivi che ti vengono mostrati in alto a destra. Buona fortuna!";
        collider.enabled = false;
        if (waitTime <= 0)
        {
            tutorialText.enabled = false;
            this.enabled = false;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }

        
    }
}
