using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AudioSubManager : MonoBehaviour
{
    [SerializeField] public TextAsset textJSON;
    private Dictionary<string, List<string>> subs;
    public void Start()
    {
        subs = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(textJSON.text);
    }


    public string GetSubs(int index, Characters type)
    {
        if (subs.TryGetValue(Globals.player.ToString() + type.ToString(), out var subtitles)) return subtitles[index];
        return null;
    }
}
