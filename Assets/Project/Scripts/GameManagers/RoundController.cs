using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class RoundController : MonoBehaviour
{
	#region Inspector Variables
	[SerializeField]
	private bool debug;
	[SerializeField]
	private bool bossRound;

	[Header("Logic")]
	[SerializeField]
	private Transform[] spawns;
	[SerializeField]
	private Transform bossSpawn;

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
	private TMP_Text roundsUIText2;

	[SerializeField]
	private GameObject rainPrefab;

	[SerializeField]
	private GameObject godRay;

	[SerializeField]
	private GameObject godRay2;
	#endregion

	#region Variables
	internal List<GameObject> currentEnemies;
	private bool finalRound;
	private bool betweenRounds=true;
	private Rounds currentRound;
	private int roundIndex;
	private Animator roundUIAnim;

	public Action OnRoundFinish;
	#endregion

	#region Life Cycle
	private void Awake()
	{
		roundUIAnim = roundsUI.GetComponent<Animator>();
		currentEnemies = new List<GameObject>();
	}

	void Start()
	{
		finalRound = false;
		roundIndex = 0;

		SelectCurrentRoundDifficulty();
		if (!debug) StartCoroutine(IStartNextRound());
	}

	void Update()
	{
		if (!debug)
		{
			if (roundIndex <= currentRound.rounds.Length) CheckCurrentEnemiesDeath();

			//if (roundIndex == currentRound.rounds.Length)
			//{
			//	bossRound = true;
			//}

			if (roundIndex > currentRound.rounds.Length)
			{
				if (!finalRound)
				{
					finalRound = true;
					betweenRounds = false;
					SetFinalRound();
				}
			}
		}
	}
	#endregion

	#region Methods
	void CheckCurrentEnemiesDeath()
	{
		if (currentEnemies.Count == 0) return;

		for (int i = 0; i < currentEnemies.Count; i++)
		{
			if (currentEnemies[i] != null) return; //Check if there is at least one enemy remaining
		}

		//If all enemies dead
		currentEnemies.Clear();
		OnRoundFinish();
		betweenRounds = true;
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
		Debug.LogWarning("Default Difficulty set: " + currentRound.players.ToString());
	}

	void SetFinalRound()
	{
		if (GameManager.Instance.selectPlayerController.GetNumPlayers() == 1)
		{
			GameManager.Instance.endGameController.CheckEndGame();
			return;
		}

		ChangeUIText("Final Round");
		roundUIAnim.SetTrigger("ChangeRound");
		if (rainPrefab!=null)
		{
			Instantiate(rainPrefab, Vector3.zero, rainPrefab.transform.rotation);
			LightIntensity.ChangeIntensityOverTime(0.5f, 2f);
			godRay.SetActive(false);
			godRay2.SetActive(false);
		}
	}

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

	void ChangeUIText(string text)
	{
		roundsUIText.text = text;
		roundsUIText2.text = text;
	}
	#endregion

	#region Getters
	public bool IsFinalRound()
	{
		return finalRound;
	}

	public bool IsBetweeenRounds()
	{
		return betweenRounds;
	}
	#endregion

	#region Coroutines
	IEnumerator IStartNextRound()
	{
		if (bossRound)
		{
			GameManager.Instance.eventsController.StopEvents();
		}

		ChangeUIText("Round: " + ToRoman(roundIndex + 1) + " / " + ToRoman(currentRound.rounds.Length + 1));
		roundUIAnim.SetTrigger("ChangeRound");
		yield return new WaitForSeconds(secondsBetweenRounds);

		coliseumAnimator.SetBool("DoorOpen", true);
		yield return new WaitForSeconds(1);

		int enemyNumberInCurrentRound = currentRound.rounds[roundIndex].enemiesInRound.Length;
		betweenRounds = false;
		for (int i = 0; i < enemyNumberInCurrentRound; i++)
		{
			if (currentRound.rounds[roundIndex].enemiesInRound[i].enemy == null) continue;
			int randomSpawn = Random.Range(0, spawns.Length);
			yield return new WaitForSeconds(secondsBetweenEnemySpawn);
			if(bossRound) currentEnemies.Add(Instantiate(currentRound.rounds[roundIndex].enemiesInRound[i].enemy, bossSpawn.position, currentRound.rounds[roundIndex].enemiesInRound[i].enemy.transform.rotation));
            else currentEnemies.Add(Instantiate(currentRound.rounds[roundIndex].enemiesInRound[i].enemy, spawns[randomSpawn].position, currentRound.rounds[roundIndex].enemiesInRound[i].enemy.transform.rotation));
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
