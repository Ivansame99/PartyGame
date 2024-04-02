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

    private void Update()
    {
        if(currentPlayersIn == expectedPlayers)
        {
            Debug.Log("Empieza a hacerse pequeño");
        }
    }

    public void Initialize(int players)
    {
        expectedPlayers = players;

        text.text = expectedPlayers.ToString() + "/" + GameManager.Instance.selectPlayerController.GetNumPlayers().ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Entras");
            currentPlayersIn++;
		}
    }

    private void OnCollisionExit(Collision collision)
    {
		if (collision.transform.CompareTag("Player"))
		{
			Debug.Log("Sales");
			currentPlayersIn--;
		}
	}
}
