using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public enum agendaType {mappa, codex};
public class ShowAgenda : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private RawImage arrowDown;
    [SerializeField] private RawImage arrowUp;
    [SerializeField] FirstPersonController fpsScript;
    [SerializeField] private Text textUp;
    [SerializeField] private Text textDown;
    public agendaType _agendaType;

    void Start()
    {
        _animator = GetComponent<Animator>();
        transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<SwitchWhatIsShowing>().enabled)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                MoveAgendaUp();
                _agendaType = agendaType.mappa;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                MoveAgendaUp();
                _agendaType = agendaType.codex;
            }
        }
        else
        {
            if((Input.GetKeyDown(KeyCode.M) && _agendaType == agendaType.mappa) || Input.GetKeyDown(KeyCode.C) && _agendaType == agendaType.codex)
            {
                MoveAgendaDown();
            }
        }
    }

    public void MoveAgendaUp()
    {
        if (_animator == null) return;
        _animator.SetBool("show", true);
    }
    public void MoveAgendaDown()
    {
        if (_animator == null) return;
        _animator.SetBool("show", false);
    }

    public void disablePlayerMovement()
    {
        fpsScript.enabled = false;
        GetComponent<SwitchWhatIsShowing>().enabled = true;

    }
    public void enablePlayerMovement()
    {
        fpsScript.enabled = true;
        GetComponent<SwitchWhatIsShowing>().enabled = false;
    }

    public void enableArrow()
    {
        textUp.enabled = true;
        textDown.enabled = true;
        arrowDown.enabled = true;
        arrowUp.enabled = true;
    }

    public void disableArrow()
    {
        arrowDown.enabled = false;
        arrowUp.enabled = false;
        textUp.enabled = false;
        textDown.enabled = false;
    }

}
