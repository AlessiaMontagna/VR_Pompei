using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters{Guardia, Schiavo, MySchiavo, Mercante, NobileM, NobileF};
public enum Players{Schiavo, Mercante, Nobile};

public static class Globals
{
    public static Players player = Players.Schiavo;
    public static bool someoneIsTalking = false;
}

// access variables via Globals.variable