using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    private float _myYPosition = 19.3f;
    private Vector3 _mission1;
    private Vector3 _mission2;
    private Vector3 _mission4;

    [SerializeField] private Transform _macellumPosition;
    [SerializeField] private Transform _mission1Position;
    [SerializeField] private Transform _mission2Position;
    [SerializeField] private Transform _mission4Position;

    private List<Vector3> _missions;
    private Vector3 _myPosition = new Vector3();
    private Vector3 _mission3 = new Vector3();


    private TextMeshProUGUI _obiettiviText;
    private string[] _textIt = {  "Nuovo obiettivo: parla con Marcus",
                                "Nuovo obiettivo: trova il tuo schiavo",
                                "Nuovo obiettivo: vai al macellum e prendi il cibo indicato",
                                "Nuovo obiettivo: vai a casa del tuo padrone"};
    private string[] _textEn = { "New goal: talk to Marcus",
                                "New goal: find your slave",
                                "New goal: go to the Macellum and take the showing food.",
                                "New goal: go to your owner's house"
                                 };
    private GameObject _pergamena;
    // Start is called before the first frame update
    void Start()
    {
        _pergamena = FindObjectOfType<Pergamena>().gameObject;
        _pergamena.SetActive(false);
        _obiettiviText = FindObjectOfType<ObiettiviText>().GetComponent<TextMeshProUGUI>();
        _mission1 = _mission1Position.position;
        _mission2 = _mission2Position.position;
        _mission3 = _macellumPosition.position;
        _mission4 = _mission4Position.position;
        _missions = new List<Vector3> {_mission1, _mission2, _mission3, _mission4 };
        //UpdateMission(Missions.Mission1_TalkWithFriend);
    }

    public void UpdateMission(Missions missionIndex)
    {
        _myPosition = _missions[(int)missionIndex];
        if(Globals.language == "en")
        {
            _obiettiviText.text = _textEn[(int)missionIndex];
        }
        else
        {
            _obiettiviText.text = _textIt[(int)missionIndex];
        }
        
        if((int)missionIndex == 2)
        {
            _pergamena.SetActive(true);
        }
        _myPosition.y = _myYPosition;
        transform.position = _myPosition;
    }

}
