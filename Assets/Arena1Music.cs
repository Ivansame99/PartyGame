using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena1Music : Music
{
	void Start()
	{
		PlayMusic("event:/MUSIC/BS-TFG_ALPHA_2");
		PlayEnvirovment("event:/SFX/Ambient/Crowd");
	}
}
