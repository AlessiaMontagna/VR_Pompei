using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters{Guardia, Schiavo, Mercante, NobileM, NobileF, Amico, MySchiavo};
public enum Players{Schiavo, Mercante, Nobile};

public static class Globals
{
    public static Players player = Players.Nobile;
    public static bool someoneIsTalking = false;
}

// access variables via Globals.variable