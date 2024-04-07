using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerCircle : MonoBehaviour
{
	[SerializeField]
	private TMP_Text text;

	[SerializeField]
	private GameObject particles;

	[SerializeField]
	private float reduceSpeed;

	private int currentPlayersIn;
	private int expectedPlayers;

	private void Start()
	{
		//Initialize(2);
		int numPlayers = GameManager.Instance.selectPlayerController.GetNumPlayers();
		expectedPlayers = Random.Range(1, numPlayers+1);
		Initialize(expectedPlayers);
	}

	private void Update()
	{
		if (currentPlayersIn == expectedPlayers)
		{
			transform.localScale -= new Vector3(reduceSpeed * Time.deltaTime, reduceSpeed * Time.deltaTime, 0f);

			if (transform.localScale.x <= 0f && transform.localScale.y <= 0f)
			{
				Debug.Log("Repartir el poder");
			}
		}
	}

	public void Initialize(int players)
	{
		//expectedPlayers = players;

		text.text = currentPlayersIn.ToString() + "/" + expectedPlayers.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			currentPlayersIn++;
			text.text = currentPlayersIn.ToString() + "/" + expectedPlayers.ToString();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			currentPlayersIn--;
			text.text = currentPlayersIn.ToString() + "/" + expectedPlayers.ToString();
		}
	}
}
