using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CodexInformation { Foro , Santuario , Macellum , ArchiOnorari , TempioDiGiove }
public enum Characters{Guardia, Soldato, Schiavo, MySchiavo, Mercante, NobileM, NobileF, Amico, SchiavoTutorial};
public enum Players{Schiavo, Mercante, Nobile};

public static class Globals
{
    public static Players player = Players.Schiavo;
    public static bool someoneIsTalking = false;
}

// access variables via Globals.variable