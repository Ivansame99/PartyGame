using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    //private PlayerHealthController[] playerHealth;
    private RoundController roundController;
    private GameObject[] players;
    private int playersCount;
    [HideInInspector]
    public int playersDead;

    void Start()
    {
        roundController = GetComponent<RoundController>();
        Invoke("GetPlayers", 1f);
    }

    public void PlayerDead()
    {
        playersDead++;
        CheckEndGame();
	}

    public void ResetPlayersDead()
    {
        playersDead = 0;
    }

	void CheckEndGame()
	{
		if (roundController.finalRound && playersDead == playersCount - 1) //Solo queda uno en pie
		{
			Debug.Log("Has ganado!!");
			Invoke(nameof(EndMatch), 5f);
		}

        if(playersDead >= playersCount)
        {
			SceneManager.LoadScene("GameOver");
		}
	}

	void EndMatch()
	{
		SceneManager.LoadScene("Win");
	}

	void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        playersCount = players.Length;
        /*playerHealth = new PlayerHealthController[playersCount];

        for (int i = 0; i < playersCount; i++)
        {
            if (players[i].GetComponent<PlayerHealthController>() != null)
            {
                playerHealth[i] = players[i].GetComponent<PlayerHealthController>();
            }
        }*/
    }
}
