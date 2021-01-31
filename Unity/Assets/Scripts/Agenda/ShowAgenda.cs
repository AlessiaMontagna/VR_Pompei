using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

enum agendaType { mappa = 0, codex = 1, obiettivi = 2 };
public class ShowAgenda : MonoBehaviour
{
    private Animator _animator;
    private bool _show = false;

    [SerializeField] private RawImage arrowDown;
    [SerializeField] private RawImage arrowUp;
    [SerializeField] FirstPersonController fpsScript;
    [SerializeField] private Text textUp;
    [SerializeField] private Text textDown;
    void Start()
    {
        _animator = GetComponent<Animator>();
        transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Agenda"))
        {
            MoveAgenda();
        }
    }

    public void MoveAgenda()
    {
        if (_animator == null) return;
        _show = !_show;
        _animator.SetBool("show", _show);
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
