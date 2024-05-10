using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class EndGameController : MonoBehaviour
{
	#region Inspector Variables
	[Header("Win Properties")]
	[SerializeField]
	private float winAnimDuration = 15f;

	[SerializeField]
	private GameObject fireworkPrefab;

	[SerializeField]
	private int maxFireworksInstanced;

	[SerializeField]
	private float minTimeFirework;

	[SerializeField]
	private float maxTimeFirework;

	[SerializeField]
	private GameObject[] coinsPrefab;

	[SerializeField]
	private float rateCoin;

	[SerializeField]
	private int totalCoinsAmmount;

	[SerializeField]
	private IdleAnimation publicAnim;

	[SerializeField]
	private Music music;

	[FMODUnity.EventRef]
	public string fireworksEventPath = "event:/SFX/Animations/Fireworks"; //cambiar evento

	#endregion

	#region Variables
	internal int playersDead = 0;
	private List<GameObject> coinsPool = new List<GameObject>();
	private float arenaLimitMin = -30f;
	private float arenaLimitMax = 30f;
	private GameManager gameManager;
	#endregion

	#region Life Cycle
	private void Awake()
	{
		gameManager = GameManager.Instance;
	}
	#endregion

	public void PlayerDead()
	{
		playersDead++;
		CheckEndGame();
	}

	public void ResetPlayersDead()
	{
		playersDead = 0;
	}

	#region Methods
	public void CheckEndGame()
	{
		int playersCount = GameManager.Instance.selectPlayerController.GetNumPlayers();

		//Check win
		if (gameManager.roundController.IsFinalRound() && playersDead == playersCount - 1) //Only one survivor
		{
			if (publicAnim != null)
			{
				publicAnim.minSimultaneousJumpAmount = publicAnim.prefabs.Length;
				publicAnim.maxSimultaneousJumpAmount = publicAnim.prefabs.Length;
			}

			gameManager.eventsController.StopEvents();
			AudioManager.Instance.ChangeToVictoryTheme();
			InstantiateCoinsPool();
			StartCoroutine(StartCoins());
			StartCoroutine(StartFireworks());
			if (music != null) music.StopMusic();
			gameManager.gmSceneManager.ChangeSceneToMenu(true, winAnimDuration);
		}

		//Check lose
		if (playersDead >= playersCount)
		{
			if (music != null) music.StopMusic();
			gameManager.gmSceneManager.ChangeSceneToGameOver(true);
		}
	}

	void InstantiateCoinsPool()
	{
		for (int i = 0; i < totalCoinsAmmount; i++)
		{
			int randomNumber = Random.Range(0, coinsPrefab.Length);
			GameObject moneda = Instantiate(coinsPrefab[randomNumber], new Vector3(0, 25f, 0), Quaternion.identity);
			moneda.SetActive(false);
			coinsPool.Add(moneda);
		}
	}

	void InstantiateCoin()
	{
		foreach (GameObject moneda in coinsPool)
		{
			if (!moneda.activeInHierarchy)
			{
				moneda.transform.position = new Vector3(Random.Range(arenaLimitMin / 2, arenaLimitMax / 2), 25f, Random.Range(arenaLimitMin / 2, arenaLimitMax / 2));
				moneda.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
				moneda.SetActive(true);
				return;
			}
		}

		GameObject firstCoint = coinsPool[0];
		firstCoint.transform.position = new Vector3(Random.Range(arenaLimitMin / 2, arenaLimitMax / 2), 25f, Random.Range(arenaLimitMin / 2, arenaLimitMax / 2));
		firstCoint.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
		firstCoint.SetActive(true);

		coinsPool.Remove(firstCoint);
		coinsPool.Add(firstCoint);
	}
	#endregion

	#region Coroutines
	private IEnumerator StartCoins()
	{
		while (true)
		{
			yield return new WaitForSeconds(rateCoin);
			InstantiateCoin();
		}
	}

	private IEnumerator StartFireworks()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minTimeFirework, maxTimeFirework));
			int randomFirework = Random.Range(1, maxFireworksInstanced);
			for (int i = 0; i < randomFirework; i++)
			{
				Vector3 randomPos = new Vector3(Random.Range(arenaLimitMin, arenaLimitMax), 0f, Random.Range(arenaLimitMin, arenaLimitMax));
				Instantiate(fireworkPrefab, randomPos, Quaternion.identity);
				FMODUnity.RuntimeManager.PlayOneShot(fireworksEventPath);
			}
		}
	}
	#endregion
}
