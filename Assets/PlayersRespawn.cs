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

	private Camera camera;
	private MultipleTargetCamera mtp;

	// Start is called before the first frame update
	void Start()
	{
		roundController = GetComponent<RoundController>();
		endGameController = GetComponent<EndGameController>();
		camera = Camera.main;
		mtp = camera.GetComponent<MultipleTargetCamera>();
		Invoke("GetPlayers", 1f);
	}

	public void SpawnPlayer(GameObject player)
	{
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
		endGameController.PlayerDead();

		StartCoroutine(SlowMotion(player));

		//Deactivate the camera to follow the player
		for (int i = 0; i < mtp.targets.Count; i++)
		{
			if (mtp.targets[i].name == player.name) mtp.targets.Remove(mtp.targets[i]);
		}
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
		camera.transform.LookAt(player);

		float zoomDistance = 120;
		camera.transform.position = player.position - camera.transform.forward * zoomDistance;

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
