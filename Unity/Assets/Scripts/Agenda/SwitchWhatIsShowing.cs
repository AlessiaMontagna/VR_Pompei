using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWhatIsShowing : MonoBehaviour
{
    private GameObject codex;
    private GameObject mappa;
    private bool m_isAxisInUse = false;
    private ShowAgenda _mappaMode;

    void Start()
    {
        _mappaMode = FindObjectOfType<ShowAgenda>();
        Codex cod = GameObject.FindObjectOfType<Codex>();
        codex = cod.gameObject;
        Mappa map = GameObject.FindObjectOfType<Mappa>();
        mappa = map.gameObject;
        this.enabled = false;
    }
    void Update()
    {
        //Map case
        if ((Input.GetKeyDown(KeyCode.M) || Input.GetAxisRaw("Arrows_v")>0) && GetComponent<ShowAgenda>()._agendaType!= agendaType.mappa)
        {
            if(m_isAxisInUse == false)  //serve per prendere l'input della freccia del joystick solo 1 volta (es.: keydown)
            {
                GetComponent<ShowAgenda>()._agendaType = agendaType.mappa;
                Show();
                m_isAxisInUse = true;
                _mappaMode.MappaMode(true);
            }
            
        }

        //Codex case
        if ((Input.GetKeyDown(KeyCode.C) || Input.GetAxisRaw("Arrows_v")<0) && GetComponent<ShowAgenda>()._agendaType != agendaType.codex)
        {
            if(m_isAxisInUse == false)
            {
                GetComponent<ShowAgenda>()._agendaType = agendaType.codex;
                Show();
                m_isAxisInUse = true;
                _mappaMode.MappaMode(false);
            }
        }
        if(Input.GetAxisRaw("Arrows_v") == 0) m_isAxisInUse = false;
    }

    private void Show()
    {
        if(GetComponent<ShowAgenda>()._agendaType == agendaType.codex)
        {
            mappa.GetComponent<Renderer>().enabled = false;
            codex.GetComponent<CodexFlip>().enabled = true;
            for (int i = 0; i < codex.transform.childCount; i++)
            {
                codex.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
            }
        }
        else 
        {
            mappa.GetComponent<Renderer>().enabled = true;
            codex.GetComponent<CodexFlip>().enabled = false;
            for (int i = 0; i < codex.transform.childCount; i++)
            {
                codex.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        codex.GetComponent<CodexFlip>().enabled = false;
        for (int i = 0; i < codex.transform.childCount; i++)
        {
            codex.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
        }
        mappa.GetComponent<Renderer>().enabled = false;
    }
}