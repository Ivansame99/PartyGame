using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SelectPlayerController : MonoBehaviour
{
	[SerializeField]
	private GameObject[] playerPos;
	[SerializeField]
	private GameObject[] prefabPlayers;
	[SerializeField]
	private GameObject[] playersUI;

	private int numPlayers;
	private GameObject[] players;

	void Awake()
	{
		Time.timeScale = 1f;

		try
		{
			var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
			numPlayers = playerConfigs.Length;
			players = new GameObject[numPlayers];
			for (int i = 0; i < playerConfigs.Length; i++)
			{
				playersUI[i].SetActive(true);
				GameObject player = Instantiate(prefabPlayers[i], playerPos[i].transform.position, playerPos[i].transform.rotation) as GameObject;
				player.name = prefabPlayers[i].name;
				player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
				players[i] = player;
				GameManager.Instance.multipleTargetCamera.AddPlayer(player.transform);
			}
		}
		catch (Exception)
		{
			SceneManager.LoadScene("HUB");
		}
	}

	public int GetNumPlayers()
	{
		return numPlayers;
	}

	public GameObject[] GetPlayers()
	{
		return players;
	}
}
