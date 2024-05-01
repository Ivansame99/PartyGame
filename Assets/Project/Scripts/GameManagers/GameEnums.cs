using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnums : MonoBehaviour
{
	public enum Scenes
	{
		Menu,
		HUB,
		Credits,
		GameOver,
		Arena1,
		ArenaSnow,
		ArenaLeaf,
	}

	public enum Arenas
	{
		None = -1,
		StandardArena = 0,
		SnowArena = 1,
		ArenaLeaf = 2,
	}
}
