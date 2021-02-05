using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Codex : MonoBehaviour
{
    public List<int> _discoveredIndex;
    // Start is called before the first frame update

    public void addDiscoveryId(int _discoveryId)
    {
        this._discoveredIndex.Add(_discoveryId);
        this._discoveredIndex.Sort();
    }
}
