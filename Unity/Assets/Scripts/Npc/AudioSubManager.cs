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

    void Start()
    {
        _subtitles.Add(Players.Nobile.ToString() + Characters.Amico.ToString(), _ConversazioneNobileAmico);
        _subtitles.Add(Players.Schiavo.ToString() + Characters.Amico.ToString(), _ConversazioneSchiavoAmico);
        _subtitles.Add(Players.Nobile.ToString() + Characters.MySchiavo.ToString(), _ConversazioneNobileSchiavo);
    }

    public string GetSubs(int index, Characters type)
    {
        if(_subtitles.TryGetValue(Globals.player.ToString()+type.ToString(), out var subtitles))return subtitles.ElementAt(index-1);
        return null;
    }
}
