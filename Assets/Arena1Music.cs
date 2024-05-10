using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena1Music : Music
{
	void Start()
	{
		PlayMusic("event:/MUSIC/BS-TFG_FINAL_BOSS");
		PlayEnvirovment("event:/SFX/Ambient/Crowd");
	}
}
