using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class AudioSubManager : MonoBehaviour
{
    [SerializeField] public TextAsset _textJSON;
    private Dictionary<string, List<string>> _subtitles = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> _audios = new Dictionary<string, List<string>>();

    private List<string> _vociMaschili = new List<string>{"Giorgio", "Francesco", "Antonio", "Klajdi", "Edoardo", "Fabrizio", "Andrea"};
    private List<string> _vociFemminili = new List<string>{"Alessia"};

    public void Start()
    {
        _subtitles = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(_textJSON.text);
        foreach (var character in new List<Characters>((Characters[])System.Enum.GetValues(typeof(Characters))))
        {
            var files = Resources.LoadAll<AudioClip>($"Talking/{character.ToString()}").ToList();
            
            _audios.Add(character.ToString()+Globals.player.ToString());
        }
    }

    public string GetSubs(int index, Characters character)
    {
        if(_subtitles.TryGetValue(Globals.player.ToString() + character.ToString(), out var subtitles)) return subtitles[index];
        return null;
    }

    public List<string> GetAudios(int index, Characters character)
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
}
