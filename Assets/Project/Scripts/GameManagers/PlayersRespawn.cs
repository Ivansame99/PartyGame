using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersRespawn : MonoBehaviour
{
	[SerializeField]
	private Transform[] spawns;

	private RoundController roundController;
	private EndGameController endGameController;

	private PlayerHealthController[] playerHealth;
	private GameObject[] players;
	private int playersCount;

	private Camera cameraMain;
	private MultipleTargetCamera mtp;

	[SerializeField]
	private bool onHub;

	// Start is called before the first frame update
	void Start()
	{
		roundController = GetComponent<RoundController>();
		endGameController = GetComponent<EndGameController>();
		cameraMain = Camera.main;
		mtp = this.GetComponent<MultipleTargetCamera>();
		Invoke("GetPlayers", 1f);
	}

	public void SpawnPlayer(GameObject player)
	{
		if (onHub)
		{
			int randomSpawn = Random.Range(0, spawns.Length);
			player.transform.position = spawns[randomSpawn].position;
			player.GetComponent<PlayerHealthController>().EnablePlayer();
			return;
		}

		if (!roundController.finalRound)
		{
			int randomSpawn = Random.Range(0, spawns.Length);
			player.transform.position = spawns[randomSpawn].position;
			player.GetComponent<PlayerHealthController>().EnablePlayer();
			mtp.targets.Add(player.transform);
		}
	}

	public void NotifyDead(Transform player)
	{

		if (onHub)
		{
			SpawnPlayer(player.gameObject);
			return;
		}
		
		endGameController.PlayerDead();

		StartCoroutine(SlowMotion(player));

		//Deactivate the camera to follow the player
		for (int i = 0; i < mtp.targets.Count; i++)
		{
			if (mtp.targets[i].name == player.name) mtp.targets.Remove(mtp.targets[i]);
		}

		player.transform.position = new Vector3(100, 10, 0);
	}

	void GetPlayers()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
		playersCount = players.Length;
		playerHealth = new PlayerHealthController[playersCount];

		for (int i = 0; i < playersCount; i++)
		{
			if (players[i].GetComponent<PlayerHealthController>() != null)
			{
				playerHealth[i] = players[i].GetComponent<PlayerHealthController>();
			}
		}
	}

	public void RespawnDeadPlayers()
	{
		for (int i = 0; i < playersCount; i++)
		{
			if (playerHealth[i].dead)
			{
				SpawnPlayer(players[i]);
			}
		}
		endGameController.ResetPlayersDead();
	}

	IEnumerator SlowMotion(Transform player)
	{
		float slowdownFactor = 0.2f;
		float slowdownDuration = 1f;

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

}
