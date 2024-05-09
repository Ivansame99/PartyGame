using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena1Music : MonoBehaviour
{
	private static FMOD.Studio.EventInstance Music;

	void Start()
	{
		Music = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/BS-TFG_FINAL_BOSS");
		Music.start();
		Music.release();
		
	}
}
