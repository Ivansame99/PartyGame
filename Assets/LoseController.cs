using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseController : MonoBehaviour
{
    private PlayerHealthController[] playerHealth;
    private EnemyDirector enemyDirector;
    private GameObject[] players;
    private int playersCount;
    public int playersDead;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetPlayers", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (players != null)
        {
            //Miramos cuantos estan muertos
            for (int i = 0; i < playersCount; i++)
            {
                if (playerHealth[i].dead) playersDead++;
            }

            //Si estan todos muertos, game over, sino se reinicia el contador
            if (playersDead == playersCount) SceneManager.LoadScene("GameOver");
            else
            {
                playersDead = 0;
            }

        }
    }

    void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        playersCount = players.Length;
        playerHealth = new PlayerHealthController[playersCount];
        //Debug.Log(playersCount);

        //playerHealth[0] = players[0].GetComponent<PlayerHealthController>();
        //playerHealth[1] = players[1].GetComponent<PlayerHealthController>();
        for (int i = 0; i < playersCount; i++)
        {
            if (players[i].GetComponent<PlayerHealthController>() != null)
            {
                playerHealth[i] = players[i].GetComponent<PlayerHealthController>();
            }
            //playerHealth[i] = players[i].GetComponent<PlayerHealthController>();
            //Debug.Log(players[i].GetComponent<PlayerHealthController>());
            //playerHealth.
        }
    }
}
