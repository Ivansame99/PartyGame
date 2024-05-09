using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Music : MonoBehaviour
{
	protected FMOD.Studio.EventInstance musicInstance;
	protected FMOD.Studio.EventInstance envirovmentInstance;

	protected void PlayMusic(string eventName)
	{
		musicInstance = FMODUnity.RuntimeManager.CreateInstance(eventName);
		musicInstance.start();
		musicInstance.release();
	}

	protected void PlayEnvirovment(string eventName)
	{
		envirovmentInstance = FMODUnity.RuntimeManager.CreateInstance(eventName);
		envirovmentInstance.start();
		envirovmentInstance.release();
	}

	// Método para detener la música
	public void StopMusic()
	{
		if (musicInstance.isValid())
		{
			musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		if (envirovmentInstance.isValid())
		{
			envirovmentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
}
