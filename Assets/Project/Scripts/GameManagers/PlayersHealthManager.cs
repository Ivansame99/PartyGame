using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHealthManager : MonoBehaviour
{
	#region Inspector Variables
	[SerializeField]
	private Transform[] playersSpawns;
	#endregion

	#region Variables
	private GameManager gameManager;
	private RoundController roundController;
	private MultipleTargetCamera mtp;

	private PlayerHealthController[] playersHealth;
	private GameObject[] players;
	private int playersCount;

	private Camera cameraMain;
	private bool onHub;
	#endregion

	#region Life Cycle
	private void Awake()
	{
		cameraMain = Camera.main;
	}

	void Start()
	{
		gameManager = GameManager.Instance;
		mtp = gameManager.multipleTargetCamera;
		onHub = gameManager.gmSceneManager.isHubScene();
		if (!onHub)
		{
			roundController = gameManager.roundController;
			roundController.OnRoundFinish += HandlePlayersWhenRoundFinish;
			GetPlayersHealths();
		}
	}
	#endregion

	public void NotifyDead(Transform player)
	{
		if (onHub)
		{
			SpawnPlayer(player.gameObject);
			return;
		}

		StartCoroutine(SlowMotion(player));

		gameManager.endGameController.PlayerDead();
		mtp.RemovePlayer(player);

		player.transform.position = new Vector3(100, 10, 0);
	}

	#region Methods
	private void SpawnPlayer(GameObject player)
	{
		if (onHub)
		{
			int randomSpawn = Random.Range(0, playersSpawns.Length);
			player.transform.position = playersSpawns[randomSpawn].position;
			player.GetComponent<PlayerHealthController>().EnablePlayer();
			return;
		}

		if (!gameManager.roundController.isFinalRound())
		{
			int randomSpawn = Random.Range(0, playersSpawns.Length);
			player.transform.position = playersSpawns[randomSpawn].position;
			player.GetComponent<PlayerHealthController>().EnablePlayer();
			mtp.AddPlayer(player.transform);
		}
	}

	private void RespawnDeadPlayers()
	{
		for (int i = 0; i < playersCount; i++)
		{
			if (playersHealth[i].dead)
			{
				SpawnPlayer(players[i]);
			}
		}
		gameManager.endGameController.ResetPlayersDead();
	}

	void GetPlayersHealths()
	{
		players = gameManager.selectPlayerController.GetPlayers();
		if (players != null)
		{
			playersCount = players.Length;
			playersHealth = new PlayerHealthController[playersCount];
			for (int i = 0; i < players.Length; i++)
			{
				playersHealth[i] = players[i].GetComponent<PlayerHealthController>();
			}
		}
	}

	private void HandlePlayersWhenRoundFinish()
	{
		RespawnDeadPlayers();
		for (int i = 0; i < playersHealth.Length; i++)
		{
			playersHealth[i].RestoreHealthAfterRound();
		}
		
	}
	#endregion

	#region Getters
	public Transform[] GetPlayersSpawns()
	{
		return playersSpawns;
	}
	#endregion

	#region Coroutines
	IEnumerator SlowMotion(Transform player)
	{
		float slowdownFactor = 0.2f;
		float slowdownDuration = 0.5f;

		Time.timeScale = slowdownFactor;
		mtp.enabled = false;

		float zoomDistance = 120;
		cameraMain.transform.position = player.position - cameraMain.transform.forward * zoomDistance;

		while (Time.timeScale < 1f)
		{
			Time.timeScale += (1f / slowdownDuration) * Time.unscaledDeltaTime;
			Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
			yield return null;
		}

		mtp.enabled = true;
		Time.timeScale = 1f;
	}
	#endregion
}
