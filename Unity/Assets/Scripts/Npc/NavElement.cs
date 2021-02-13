using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NavRoles{Spawn, Stop, Path};
public enum NavSubroles{PeopleSpawn, FlocksSpawn, 
    GuardStop, MercanteStop, BalconyStop, GroupStop,
    ColonnatoPath, ForoPath, ViaPath, MacellumPath, TempioPath};

public class NavElement : MonoBehaviour
{
    [SerializeField] private NavRoles _role;
    [SerializeField] private NavSubroles _subrole;
    // Start is called before the first frame update
    void Start(){}
    public NavRoles GetRole(){return _role;}
    public NavSubroles GetSubrole(){return _subrole;}
}