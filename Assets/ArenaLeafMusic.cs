using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLeafMusic : MonoBehaviour
{
	private static FMOD.Studio.EventInstance Music;

	void Start()
	{
		Music = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/BS-TFG_FINAL_ARENA_NIEVE");
		Music.start();
		Music.release();

	}
}
