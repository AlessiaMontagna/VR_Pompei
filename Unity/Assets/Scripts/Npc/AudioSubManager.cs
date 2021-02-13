using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class AudioSubManager : MonoBehaviour
{
    [SerializeField] public TextAsset _textJSON;
    private Dictionary<string, List<string>> _subtitles = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> _voices = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> _audios = new Dictionary<string, List<string>>();

    public void Awake()
    {
        _subtitles = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(_textJSON.text);
        foreach (var character in new List<Characters>((Characters[])System.Enum.GetValues(typeof(Characters))))
        {
            var characterFiles = Resources.LoadAll<AudioClip>($"Talking/{character.ToString()}").ToList();
            foreach (var player in new List<Players>((Players[])System.Enum.GetValues(typeof(Players))))
            {
                var key = player.ToString()+character.ToString();
                foreach (var file in characterFiles.Where(i => i.name.Contains(key)))
                {
                    var voice = file.name.Split('_')[1];
                    if(_voices.TryGetValue(key, out var voices)){if(!voices.Contains(voice))voices.Add(voice);}
                    else _voices.Add(key, new List<string>{voice});
                    if(_audios.TryGetValue(key+voice, out var audios)) audios.Add($"Talking/{character.ToString()}/{file.name}");
                    else _audios.Add(key+voice, new List<string>{$"Talking/{character.ToString()}/{file.name}"});
                }
            }
        }
    }

    public string GetVoice(Characters character)
    {
        if(!_voices.TryGetValue(Globals.player.ToString() + character.ToString(), out var voices)) return null;
        return voices.ElementAt(Random.Range(0, voices.Count));
    }

    public string GetSubs(int index, Characters character)
    {
        if(!_subtitles.TryGetValue(Globals.player.ToString() + character.ToString(), out var subtitles)) return null;
        return subtitles.ElementAt(index);
    }

    public string GetAudio(int index, Characters character, string voice)
    {
        if(!_audios.TryGetValue(Globals.player.ToString() + character.ToString() + voice, out var audios))return null;
        foreach (var audio in audios) if(audio.Contains(index.ToString()))return audio;
        return null;
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
