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

	[Header("Circle Transition")]
	[SerializeField]
	private Material transitionMaterial;
	[SerializeField]
	private float transitionTime = 2f;
	[SerializeField]
	private string propertyName = "_Progress";

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
			StartCoroutine(CloseTranitionToWin());
		}

        if(playersDead >= playersCount)
        {
			StartCoroutine(CloseTranitionToGameOver());
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
    }

	private IEnumerator CloseTranitionToWin()
	{
		float currentTime = transitionTime;
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
			yield return null;
		}

		SceneManager.LoadScene("Win");
	}

	private IEnumerator CloseTranitionToGameOver()
	{
		float currentTime = transitionTime;
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			transitionMaterial.SetFloat(propertyName, Mathf.Clamp01(currentTime / transitionTime));
			yield return null;
		}

		SceneManager.LoadScene("GameOver");
	}
}
