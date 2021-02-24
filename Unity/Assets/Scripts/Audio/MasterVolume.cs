using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolume : MonoBehaviour
{
	FMOD.Studio.Bus _talkBus;
	FMOD.Studio.Bus _sfxBus;
	FMOD.Studio.Bus _masterBus;

	void Start()
	{
        _talkBus = FMODUnity.RuntimeManager.GetBus("bus:/Talk");
        _masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
	}

	void Update()
	{
        AudioListener.volume = Globals.masterVolume;
		_masterBus.setVolume(Globals.masterVolume);
		_talkBus.setVolume(Globals.talkVolume);
		_sfxBus.setVolume(Globals.sfxVolume);
	}
}
