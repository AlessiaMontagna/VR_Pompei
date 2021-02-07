using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWhatIsShowing : MonoBehaviour
{

    private GameObject codex;
    private GameObject mappa;
    private GameObject obiettivi;
    private bool m_isAxisInUse = false;
    public byte indexAgenda;
    [SerializeField] private Text textUp;
    [SerializeField] private Text textDown;
    // Start is called before the first frame update
    void Start()
    {
        Codex cod = GameObject.FindObjectOfType<Codex>();
        codex = cod.gameObject;
        Mappa map = GameObject.FindObjectOfType<Mappa>();
        mappa = map.gameObject;
        Obiettivi obs = GameObject.FindObjectOfType<Obiettivi>();
        obiettivi = obs.gameObject;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxisRaw("Arrows_v")>0)
        {
            if(m_isAxisInUse == false)  //serve per prendere l'input della freccia del joystick solo 1 volta (es.: keydown)
         {
            if (indexAgenda == 0) indexAgenda = 2; else indexAgenda--;
            Show();
            m_isAxisInUse = true;
         }
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Arrows_v")<0)
        {
            if(m_isAxisInUse == false)
            {
            if (indexAgenda == 2) indexAgenda = 0; else indexAgenda++;
            Show();
            m_isAxisInUse = true;
            }
        }

        if(Input.GetAxisRaw("Arrows_v") == 0) m_isAxisInUse = false;
    }

    private void Show()
    {

        if (indexAgenda == (int)agendaType.codex)
        {
            mappa.GetComponent<Mappa>().enabled = false;
            mappa.GetComponent<Renderer>().enabled = false;
            obiettivi.GetComponent<Renderer>().enabled = false;
            codex.GetComponent<CodexFlip>().enabled = true;
            for (int i = 0; i < 6; i++)
            {
                codex.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
            }
            textDown.text = "Obiettivi";
            textUp.text = "Mappa";


        }
        else if (indexAgenda == (int)agendaType.mappa)
        {
            mappa.GetComponent<Mappa>().enabled = true;
            mappa.GetComponent<MeshRenderer>().enabled = true;
            obiettivi.GetComponent<Renderer>().enabled = false;
            codex.GetComponent<CodexFlip>().enabled = false;
            for (int i = 0; i < 6; i++)
            {
                codex.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            }
            textDown.text = "Codex";
            textUp.text = "Obiettivi";
        }
        else
        {
            mappa.GetComponent<Mappa>().enabled = false;
            mappa.GetComponent<Renderer>().enabled = false;
            obiettivi.GetComponent<Renderer>().enabled = true;
            codex.GetComponent<CodexFlip>().enabled = false;
            for (int i = 0; i < 6; i++)
            {
                codex.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            }
            textDown.text = "Mappa";
            textUp.text = "Codex";
        }
    }

    private void OnDisable()
    {
        codex.GetComponent<CodexFlip>().enabled = false;
        for (int i = 0; i < 6; i++)
        {
            codex.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
        }
        mappa.GetComponent<Mappa>().enabled = false;
        mappa.GetComponent<Renderer>().enabled = false;
        obiettivi.GetComponent<Renderer>().enabled = false;
        //textUp.text = "";
        //textDown.text = "";
    }
}