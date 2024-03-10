using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum NumPlayers
{
	OnePlayer = 1,
	TwoPlayers = 2,
	ThreePlayers = 3,
	FourPlayers = 4
}

[System.Serializable]
public struct EnemyWithPower
{
	public GameObject enemy;
	public int power;
}

[System.Serializable]
public class RoundEnemies
{
	public EnemyWithPower[] enemiesInRound;
}

[CreateAssetMenu(menuName = "Rounds")]
public class Rounds : ScriptableObject
{
    public NumPlayers players;
	public RoundEnemies[] rounds;
}
