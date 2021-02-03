using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioSubManager : MonoBehaviour
{
    private List<string> _subSchiavoGuardia = new List<string>{"Ciao, come va?"};
    private List<string> _subSchiavoSchiavo = new List<string>{"Ciao, come va?"};
    private List<string> _subSchiavoMercante = new List<string>{"Ciao, come va?"};
    private List<string> _subSchiavoNobileM = new List<string>{"Ciao, come va?"};
    private List<string> _subSchiavoNobileF = new List<string>{"Ciao, come va?"};
    
    private List<string> _subMercanteGuardia = new List<string>{"Ciao, come va?"};
    private List<string> _subMercanteSchiavo = new List<string>{"Buongiorno desideri qualcosa?",
                                            "Buongiorno oggi mi sento molto generoso, per questo pane posso farti un buon prezzo",
                                            "Buongiorno cerchi qualcosa in particolare?"} ;
    private List<string> _subMercanteMercante = new List<string>{"Ciao, come va?"};
    private List<string> _subMercanteNobileM = new List<string>{"Buongiorno Signore vede qualcosa che l'aggrada?",
                                            "Buongiorno mio Signore, vede qualcosa che le possa interessare?",
                                            "Ma Buongiorno mio signore! Ogggi Nettuno ci ha donato una ricca pesca" };
    private List<string> _subMercanteNobileF = new List<string>{"Ciao, come va?"};
    
    private List<string> _subNobileGuardia = new List<string>{"Ciao, come va?"};
    private List<string> _subNobileSchiavo = new List<string>{"Ciao, come va?"};
    private List<string> _subNobileMercante = new List<string>{"Ciao, come va?"};
    private List<string> _subNobileNobileM = new List<string>{"Ciao, come va?"};
    private List<string> _subNobileNobileF = new List<string>{"Ciao, come va?"};

    private Dictionary<string, List<string>> _subtitles = new Dictionary<string, List<string>>();

    void Start()
    {
        _subtitles.Add(Players.Schiavo.ToString()+Characters.Guardia.ToString(), _subSchiavoGuardia);
        _subtitles.Add(Players.Schiavo.ToString()+Characters.Schiavo.ToString(), _subSchiavoSchiavo);
        _subtitles.Add(Players.Schiavo.ToString()+Characters.Mercante.ToString(), _subSchiavoMercante);
        _subtitles.Add(Players.Schiavo.ToString()+Characters.NobileM.ToString(), _subSchiavoNobileM);
        _subtitles.Add(Players.Schiavo.ToString()+Characters.NobileF.ToString(), _subSchiavoNobileF);
        
        _subtitles.Add(Players.Mercante.ToString()+Characters.Guardia.ToString(), _subMercanteGuardia);
        _subtitles.Add(Players.Mercante.ToString()+Characters.Schiavo.ToString(), _subMercanteSchiavo);
        _subtitles.Add(Players.Mercante.ToString()+Characters.Mercante.ToString(), _subMercanteMercante);
        _subtitles.Add(Players.Mercante.ToString()+Characters.NobileM.ToString(), _subMercanteNobileM);
        _subtitles.Add(Players.Mercante.ToString()+Characters.NobileF.ToString(), _subMercanteNobileF);

        _subtitles.Add(Players.Nobile.ToString()+Characters.Guardia.ToString(), _subNobileGuardia);
        _subtitles.Add(Players.Nobile.ToString()+Characters.Schiavo.ToString(), _subNobileSchiavo);
        _subtitles.Add(Players.Nobile.ToString()+Characters.Mercante.ToString(), _subNobileMercante);
        _subtitles.Add(Players.Nobile.ToString()+Characters.NobileM.ToString(), _subNobileNobileM);
        _subtitles.Add(Players.Nobile.ToString()+Characters.NobileF.ToString(), _subNobileNobileF);
    }

    public string GetSubs(int index, Characters type)
    {
        if(_subtitles.TryGetValue(Globals.player.ToString()+type.ToString(), out var subtitles))return subtitles.ElementAt(index-1);
        return null;
    }
}
