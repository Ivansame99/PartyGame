using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaLeafMusic : Music
{
	void Start()
	{
		PlayMusic("event:/MUSIC/BS-TFG_ALPHA_2");
		PlayEnvirovment("event:/SFX/Ambient/Jungle");
		PlayEnvirovment2("event:/SFX/Ambient/Crowd");
	}
}
