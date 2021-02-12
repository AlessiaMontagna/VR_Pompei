using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class AudioSubManager : MonoBehaviour
{
    [SerializeField] public TextAsset _textJSON;
    private Dictionary<string, List<string>> _subtitles;

    private List<string> _vociMaschili = new List<string>{"Giorgio", "Francesco", "Antonio", "Klajdi", "Edoardo", "Fabrizio"};
    private List<string> _vociFemminili = new List<string>{"Alessia"};

    public void Start()
    {
        _subtitles = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(_textJSON.text);
    }

    public string GetSubs(int index, Characters type)
    {
        if (_subtitles.TryGetValue(Globals.player.ToString() + type.ToString(), out var subtitles)) return subtitles[index];
        return null;
    }

    public List<string> GetAudios(Characters character)
    {
        var files = Resources.LoadAll<AudioClip>($"Talking/{character.ToString()}").ToList();
        var voci = _vociMaschili;
        if(character == Characters.NobileF)voci = _vociFemminili;
        while(voci.Count > 0){
            var voce = voci.ElementAt(Random.Range(0, voci.Count));
            voci.Remove(voce);
            var tmp = files.Where(i => i.name.Contains(voce)).ToList();
            if(tmp.Count > 0) {files = tmp;break;}
        }
        if(files.Count < 3) Debug.LogError($"Not enough files found for {character.ToString()} ({files.Count}/3)");
        List<string> audios = new List<string>();
        foreach (var item in files){audios.Add(item.name.Split('_')[1]);}
        return audios;
    }

    public int GetMaxAudios(Characters characterType)
    {
        if (_subtitles.TryGetValue(Globals.player.ToString() + characterType.ToString(), out var subtitles))
        {
            return subtitles.Count;
        }
        return -1;
    }
}
