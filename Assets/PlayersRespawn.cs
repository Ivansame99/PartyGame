using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayersRespawn : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawns;

    private RoundController roundController;

    private bool onlyOnce=false;

    private PlayerHealthController[] playerHealth;
    private GameObject[] players;
    private int playersCount;

    // Start is called before the first frame update
    void Start()
    {
        roundController = GetComponent<RoundController>();
        Invoke("GetPlayers", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!onlyOnce && roundController.finalRound)
        {
            for (int i = 0; i < playersCount; i++)
            {
                if (playerHealth[i].dead)
                {
                    SpawnPlayer(players[i]);
                }
            }
            onlyOnce = true;
        }
    }

    public void SpawnPlayer(GameObject player)
    {
        if (!roundController.finalRound || !onlyOnce)
        {
            int randomSpawn = Random.Range(0, spawns.Length);
            player.transform.position = spawns[randomSpawn].position;
            player.GetComponent<PlayerHealthController>().EnablePlayer();
        }
    }

    public void NotifyDead()
    {
        if (roundController.finalRound)
        {
            roundController.playersDied++;
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
}
