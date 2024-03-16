using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SelectPlayerController : MonoBehaviour
{
	#region Inspector Variables
	[SerializeField]
	private GameObject[] playerPos;
	[SerializeField]
	private GameObject[] prefabPlayers;
	[SerializeField]
	private GameObject[] playersUI;
	#endregion

	#region Variables
	private int numPlayers;
	private GameObject[] players;
	#endregion

	#region Life Cycle
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
	#endregion

	#region Getters
	public int GetNumPlayers()
	{
		return numPlayers;
	}

	public GameObject[] GetPlayers()
	{
		return players;
	}
	#endregion
}
