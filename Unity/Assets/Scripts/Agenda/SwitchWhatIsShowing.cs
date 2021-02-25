using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWhatIsShowing : MonoBehaviour
{
    private GameObject codex;
    private GameObject mappa;
    private ShowAgenda _showAgenda;

    void Start()
    {
        _showAgenda = FindObjectOfType<ShowAgenda>();
        Codex cod = GameObject.FindObjectOfType<Codex>();
        codex = cod.gameObject;
        Mappa map = GameObject.FindObjectOfType<Mappa>();
        mappa = map.gameObject;
        this.enabled = false;
    }
    void Update()
    {
        //Map case
        if ((Input.GetButtonDown("Map") ) && GetComponent<ShowAgenda>()._agendaType!= agendaType.mappa && !Globals.gamePause)
        {
            GetComponent<ShowAgenda>()._agendaType = agendaType.mappa;
            Show();
            _showAgenda.MappaMode(true);
        }

        if ((Input.GetButtonDown("Codex") ) && GetComponent<ShowAgenda>()._agendaType != agendaType.codex && !Globals.gamePause && GetComponentInChildren<Codex>()._discoveredIndex.Count > 0)
        {
            GetComponent<ShowAgenda>()._agendaType = agendaType.codex;
            Show();
            FindObjectOfType<CodexText>().GetComponent<TextMeshProUGUI>().text = "";
            _showAgenda.MappaMode(false);
        }
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