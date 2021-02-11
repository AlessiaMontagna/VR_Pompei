using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private float _myYPosition = 19.3f;
    private Vector3 _mission1;
    private Vector3 _mission2;
    [SerializeField] private Transform _macellumPosition;
    private List<Vector3> _missions;
    private Vector3 _myPosition = new Vector3();
    private Vector3 _mission3 = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        _mission1 =  FindObjectOfType<AmicoInteractable>().transform.position;
        _mission2 = FindObjectOfType<MySchiavoInteractable>().transform.position;
        _mission3 = _macellumPosition.position;
        _missions = new List<Vector3> {_mission1, _mission2, _mission3 };
        UpdateMission(Missions.Mission1_TalkWithFriend);
    }

    public void UpdateMission(Missions missionIndex)
    {
        _myPosition = _missions[(int)missionIndex];
        _myPosition.y = _myYPosition;
        transform.position = _myPosition;
    }

}
