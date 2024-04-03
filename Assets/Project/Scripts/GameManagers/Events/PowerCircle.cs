using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerCircle : MonoBehaviour
{
	[SerializeField]
	private TMP_Text text;

	private int currentPlayersIn;
	private int expectedPlayers;

	private void Start()
	{
		Initialize(2);
	}

	private void Update()
	{
		if (currentPlayersIn == expectedPlayers)
		{
			Debug.Log("Empieza a hacerse pequeño");
		}
	}

	public void Initialize(int players)
	{
		expectedPlayers = players;

		//text.text = expectedPlayers.ToString() + "/" + GameManager.Instance.selectPlayerController.GetNumPlayers().ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Entras");
			currentPlayersIn++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Sales");
			currentPlayersIn--;
		}
	}
}
