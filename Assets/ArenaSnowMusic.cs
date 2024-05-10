using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSnowMusic : Music
{
	void Start()
	{
		PlayMusic("event:/MUSIC/BS-TFG_FINAL_ARENA_NIEVE");
		PlayEnvirovment("event:/SFX/Ambient/Crowd");
	}
}
