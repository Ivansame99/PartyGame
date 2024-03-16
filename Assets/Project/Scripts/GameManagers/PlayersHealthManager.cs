using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHealthManager : MonoBehaviour
{
	[SerializeField]
	private Transform[] playersSpawns;

	[SerializeField]
	private bool onHub;

	private GameManager gameManager;
	private MultipleTargetCamera mtp;

	private PlayerHealthController[] playersHealth;
	private GameObject[] players;
	private int playersCount;

	private Camera cameraMain;

	private void Awake()
	{
		cameraMain = Camera.main;
	}

	void Start()
	{
		gameManager = GameManager.Instance;
		mtp = gameManager.multipleTargetCamera;
		if (!onHub) GetPlayersHealths();
	}

	public void SpawnPlayer(GameObject player)
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

	public void NotifyDead(Transform player)
	{

		if (onHub)
		{
			SpawnPlayer(player.gameObject);
			return;
		}

		gameManager.endGameController.PlayerDead();

		StartCoroutine(SlowMotion(player));

		//Deactivate the camera to follow the player
		for (int i = 0; i < mtp.targets.Count; i++)
		{
			if (mtp.targets[i].name == player.name) mtp.targets.Remove(mtp.targets[i]);
		}

		player.transform.position = new Vector3(100, 10, 0);
	}

	public void RespawnDeadPlayers()
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
		playersCount = players.Length;
		playersHealth = new PlayerHealthController[playersCount];
		for (int i = 0; i < players.Length; i++)
		{
			playersHealth[i] = players[i].GetComponent<PlayerHealthController>();
		}
	}

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
