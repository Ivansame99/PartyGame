using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMusic : Music
{   
    void Start()
    {
		PlayMusic("event:/MUSIC/BS-TFG_entradilla_mazmorra");
		//PlayEnvirovment("event:/MUSIC/BS-TFG_FINAL_ARENA_NIEVE");
	}
}
