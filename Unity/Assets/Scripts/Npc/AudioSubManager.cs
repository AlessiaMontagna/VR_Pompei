using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioSubManager : MonoBehaviour
{
    private List<string> _ConversazioneNobileAmico = new List<string> { "Buongiorno Caius. Come accordato, ci vediamo stasera a cena.", "Hai chiesto al tuo servo di prendere qualcosa da mangiare stasera?"};
    private List<string> _ConversazioneSchiavoAmico = new List<string> { "Vai a comprare il cibo al mercato.", "Cosa fai ancora quì? Fai ciò che ti ha detto il tuo padrone", "Scegli bene il cibo che devi prendere, servo"};
    private List<string> _ConversazioneNobileSchiavo = new List<string> { "Va bene, capo","Buongiorno mio padrone, vorrei sapere che cosa vuole da me","Vai al macellum","A prendere del pesce" };

    private Dictionary<string, List<string>> _subtitles = new Dictionary<string, List<string>>();

    private List<string> _vociMaschili = new List<string>{"Giorgio", "Francesco", "Antonio", "Klajdi", "Edoardo", "Fabrizio"};
    private List<string> _vociFemminili = new List<string>{"Alessia"};

    void Start()
    {
        _subtitles.Add(Players.Nobile.ToString() + Characters.Amico.ToString(), _ConversazioneNobileAmico);
        _subtitles.Add(Players.Schiavo.ToString() + Characters.Amico.ToString(), _ConversazioneSchiavoAmico);
        _subtitles.Add(Players.Nobile.ToString() + Characters.MySchiavo.ToString(), _ConversazioneNobileSchiavo);
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
        if(files.Count < 3) Debug.LogError($"Not enough files found for {character.ToString()}");
        List<string> audios = new List<string>();
        foreach (var item in files){audios.Add(item.name.Split('_')[1]);}
        return audios;
    }

    public string GetSubs(int index, Characters type)
    {
        if(_subtitles.TryGetValue(Globals.player.ToString()+type.ToString(), out var subtitles))return subtitles.ElementAt(index);
        return null;
    }
}
