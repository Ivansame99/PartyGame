using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class RoundController : MonoBehaviour
{
	#region Inspector Variables
	[SerializeField]
	private bool debug;

	[Header("Logic")]
	[SerializeField]
	private Transform[] spawns;

	[SerializeField]
	private Rounds[] roundsSO;

	[SerializeField]
	private float secondsBetweenRounds;

	[SerializeField]
	private float secondsBetweenEnemySpawn;

	[Header("Visual feedback")]
	[SerializeField]
	private Animator coliseumAnimator;

	[SerializeField]
	private GameObject roundsUI;

	[SerializeField]
	private TMP_Text roundsUIText;

	[SerializeField]
	private GameObject rainPrefab;

	[SerializeField]
	private GameObject godRay;

	[SerializeField]
	private GameObject godRay2;
	#endregion

	internal List<GameObject> currentEnemies;
	private bool finalRound;
	private Rounds currentRound;
	private int roundIndex;
	private GameObject[] players;
	private PlayerHealthController[] playersHealth;
	private Animator roundUIAnim;
	private PlayersHealthManager playersHealthManager;

	private void Awake()
	{
		roundUIAnim = roundsUI.GetComponent<Animator>();
		playersHealthManager = GameManager.Instance.playersHealthManager;
	}

	void Start()
	{
		finalRound = false;
		roundIndex = 0;
		currentEnemies = new List<GameObject>();

		SelectCurrentRoundDifficulty();
		if (!debug) StartCoroutine(IStartNextRound());
		GetPlayers();
	}

	void Update()
	{
		if (!debug)
		{
			if (roundIndex <= currentRound.rounds.Length) CheckCurrentEnemiesDeath();
			else
			{
				if (!finalRound)
				{
					SetFinalRound();
					finalRound = true;
				}
			}
		}
	}

	void CheckCurrentEnemiesDeath()
	{
		if (currentEnemies.Count == 0) return;

		for (int i = 0; i < currentEnemies.Count; i++)
		{
			if (currentEnemies[i] != null) return; //Check if there is at least one enemy remaining
		}

		//If all enemies dead
		currentEnemies.Clear();
		playersHealthManager.RespawnDeadPlayers();
		for(int i= 0;i< playersHealth.Length; i++)
		{
			playersHealth[i].RestoreHealthAfterRound();
		}

		//Next round
		if (roundIndex < currentRound.rounds.Length) StartCoroutine(IStartNextRound());
		else if (roundIndex == currentRound.rounds.Length) roundIndex++;
	}

	private void SelectCurrentRoundDifficulty()
	{
		int numplayers = GameManager.Instance.selectPlayerController.GetNumPlayers();
		for (int i = 0; i < roundsSO.Length; i++)
		{
			if ((int)roundsSO[i].players == numplayers)
			{
				currentRound = roundsSO[i];
				if(currentRound!=null) Debug.Log("Round Difficulty: " + currentRound.players.ToString() );
				return;
			}
		}
		currentRound = roundsSO[0];
		Debug.LogError("Default Difficulty set: " + currentRound.players.ToString());
	}

	void SetFinalRound()
	{
		roundsUIText.text = "Final Round";
		roundUIAnim.SetTrigger("ChangeRound");
		Instantiate(rainPrefab, Vector3.zero, rainPrefab.transform.rotation);
		LightIntensity.ChangeIntensityOverTime(0.5f, 2f);
		godRay.SetActive(false);
		godRay2.SetActive(false);
	}

	// Integer to Roman numerals - mgear - http://unitycoder.com/blog/
	// Unity3D version converted from this: http://rosettacode.org/wiki/Roman_numerals/Encode
	string ToRoman(int number)
	{
		if (number < 1) return string.Empty;
		if (number >= 1000) return "M" + ToRoman(number - 1000);
		if (number >= 900) return "CM" + ToRoman(number - 900);
		if (number >= 500) return "D" + ToRoman(number - 500);
		if (number >= 400) return "CD" + ToRoman(number - 400);
		if (number >= 100) return "C" + ToRoman(number - 100);
		if (number >= 90) return "XC" + ToRoman(number - 90);
		if (number >= 50) return "L" + ToRoman(number - 50);
		if (number >= 40) return "XL" + ToRoman(number - 40);
		if (number >= 10) return "X" + ToRoman(number - 10);
		if (number >= 9) return "IX" + ToRoman(number - 9);
		if (number >= 5) return "V" + ToRoman(number - 5);
		if (number >= 4) return "IV" + ToRoman(number - 4);
		if (number >= 1) return "I" + ToRoman(number - 1);
		return "Impossible state reached";
	}

	void GetPlayers()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
		playersHealth = new PlayerHealthController[players.Length];

		for(int i=0;i< players.Length; i++)
		{
			playersHealth[i] = players[i].GetComponent<PlayerHealthController>();
		}
	}

	#region Getters
	public bool isFinalRound()
	{
		return finalRound;
	}
	#endregion

	#region Coroutines
	IEnumerator IStartNextRound()
	{
		roundsUIText.text = "Round " + ToRoman(roundIndex + 1);
		roundUIAnim.SetTrigger("ChangeRound");
		yield return new WaitForSeconds(secondsBetweenRounds);

		coliseumAnimator.SetBool("DoorOpen", true);
		yield return new WaitForSeconds(1);

		int enemyNumberInCurrentRound = currentRound.rounds[roundIndex].enemiesInRound.Length;

		for (int i = 0; i < enemyNumberInCurrentRound; i++)
		{
			int randomSpawn = Random.Range(0, spawns.Length);
			yield return new WaitForSeconds(secondsBetweenEnemySpawn);
			currentEnemies.Add(Instantiate(currentRound.rounds[roundIndex].enemiesInRound[i].enemy, spawns[randomSpawn].position, currentRound.rounds[roundIndex].enemiesInRound[i].enemy.transform.rotation));
			StartCoroutine(ChangePowerLevel(roundIndex, i));
		}
		coliseumAnimator.SetBool("DoorOpen", false);
		roundIndex++;
	}

	IEnumerator ChangePowerLevel(int roundIndexParameter, int indexEnemy)
	{
		yield return new WaitForSeconds(0.1f); //Time between spawn the enemy and change his power
		currentEnemies[indexEnemy].GetComponent<PowerController>().InitializePowerLevel(currentRound.rounds[roundIndexParameter].enemiesInRound[indexEnemy].power);
	}
	#endregion
}
