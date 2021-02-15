using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NavRoles{Spawn, Stop, Path};
public enum NavSubroles{PeopleSpawn, FlocksSpawn, 
    GuardStop, SoldierStop, MercanteStop, BalconyStop, GroupStop,
    ColonnatoPath, ForoPath, ViaPath, MacellumPath, TempioPath};

public class NavElement : MonoBehaviour
{
    [SerializeField] private NavRoles _role;
    public NavRoles role => _role;
    [SerializeField] private NavSubroles _subrole;
    public NavSubroles subrole => _subrole;
    [SerializeField] private MercanteFoodTypes _foodType;
    public MercanteFoodTypes foodType => _foodType;
}