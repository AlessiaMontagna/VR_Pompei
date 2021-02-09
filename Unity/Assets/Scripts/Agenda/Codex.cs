using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Codex : MonoBehaviour
{
    public List<int> _discoveredIndex;
    public int _currentPage = 0;

    // Start is called before the first frame update

    public void addDiscoveryId(int _discoveryId)
    {
        
        this._discoveredIndex.Add(_discoveryId);
        this._discoveredIndex.Sort();
        if(_discoveryId > this._discoveredIndex.Count-1) this._currentPage = this._discoveredIndex.Count-1;
        else this._currentPage = _discoveryId;
    }
}
