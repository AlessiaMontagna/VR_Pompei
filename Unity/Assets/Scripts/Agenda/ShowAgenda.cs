using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
public enum agendaType {mappa, codex};

[RequireComponent(typeof(AudioSource))]
public class ShowAgenda : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject _projectCamera;
    [SerializeField] private GameObject _redPoint;

    private InteractionManager _interactionScript;

    public agendaType _agendaType;
    private AudioSource _audioSource;

    public Image _leftArrow;
    public Image _rightArrow;

    void Start()
    {
        _leftArrow = FindObjectOfType<UILeft>().GetComponent<Image>();
        _rightArrow = FindObjectOfType<UIRight>().GetComponent<Image>();

        _interactionScript = FindObjectOfType<InteractionManager>();
        MappaMode(false);
        _audioSource = GetComponent<AudioSource>();
        //_audioSource.clip = Resources.Load<AudioClip>("FeedbackSounds/Agenda_Up_Down");
        _animator = GetComponent<Animator>();
        transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Globals.gamePause)
        {
            if (!GetComponent<SwitchWhatIsShowing>().enabled)
            {
                if (Input.GetButton("Map")/*Input.GetKeyDown(KeyCode.V)*/)
                {
                    MappaMode(true);
                    MoveAgendaUp();
                    _agendaType = agendaType.mappa;
                }
                else if (Input.GetButton("Codex") && GetComponentInChildren<Codex>()._discoveredIndex.Count > 0)/*Input.GetKeyDown(KeyCode.C)*/)
                {
                    MappaMode(false);
                    MoveAgendaUp();
                    _agendaType = agendaType.codex;
                    FindObjectOfType<CodexText>().GetComponent<TextMeshProUGUI>().text = ""; // tolgo "Nuova voce nel codex"
                }
            }
            else
            {
                if ((Input.GetButton("Map")/*Input.GetKeyDown(KeyCode.V)*/ && _agendaType == agendaType.mappa) || Input.GetButton("Codex")/*Input.GetKeyDown(KeyCode.C)*/ && _agendaType == agendaType.codex)
                {
                    MoveAgendaDown();
                    MappaMode(false);
                }
            }
        }
        
    }

    public void MappaMode(bool value)
    {
        _projectCamera.SetActive(value);
        _redPoint.GetComponent<Animator>().enabled = value;
    }

    public void MoveAgendaUp()
    {
        if (_animator == null) return;
        _animator.SetBool("show", true);
        _audioSource.clip = Resources.Load<AudioClip>("FeedbackSounds/Agenda_Up");
        _audioSource.Play();
    }
    public void MoveAgendaDown()
    {
        if (_animator == null) return;
        _animator.SetBool("show", false);
        _audioSource.clip = Resources.Load<AudioClip>("FeedbackSounds/Agenda_Down");
        _audioSource.Play();
    }

    public void disablePlayerMovement()
    {
        //fpsScript.enabled = false;
        _interactionScript.enabled = false;
        GetComponent<SwitchWhatIsShowing>().enabled = true;

    }
    public void enablePlayerMovement()
    {
        _interactionScript.enabled = true;
        GetComponent<SwitchWhatIsShowing>().enabled = false;
    }

    public void enableArrow()
    {
        
        
    }

    public void disableArrow()
    {
        if (_agendaType == agendaType.codex)
        {
            _leftArrow.enabled = false;
            _rightArrow.enabled = false;
        }
    }

    public void LeftArrow(bool value)
    {
        _leftArrow.enabled = value;
    }
    public void RightArrow(bool value)
    {
        _rightArrow.enabled = value;
    }

}
