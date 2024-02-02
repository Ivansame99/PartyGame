using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyTarget : MonoBehaviour
{
    private EnemyDirector enemyDirector;

    //Targets
    private bool newTarget;
    private Transform lastTarget;

    public Transform player, player2;

    // Update is called once per frame
    void Update()
    {
        player = FindPlayer();
        player2 = FindSecondClosestPlayer(player);
    }

    private Transform FindPlayer()
    {
        Transform searchPlayer = null;
        float minDist = float.MaxValue;

        for (int i = 0; i < enemyDirector.currentPlayers; i++)
        {
            Transform player = enemyDirector.players[i];
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < minDist && player.GetComponent<PlayerHealthController>().dead == false)
            {
                if (enemyDirector.full[i] && player == lastTarget)
                {
                    minDist = distance;
                    searchPlayer = player;
                }
                if (!enemyDirector.full[i])
                {
                    minDist = distance;
                    searchPlayer = player;
                }
            }
        }
        if (!newTarget)
        {
            // Disminuir el contador del objetivo anterior si ya estaba siguiendo a otro jugador
            // Incrementar el contador del nuevo objetivo
            IncreasePlayerTarget(searchPlayer.gameObject.name);

            lastTarget = searchPlayer;
            newTarget = true;
        }
        if (lastTarget != null && lastTarget != searchPlayer)
        {
            DecreasePlayerTarget(lastTarget.gameObject.name);
            newTarget = false;
        }
        /*
        if(enemyHealth.dead == true)
        {
            DecreasePlayerTarget(searchPlayer.gameObject.name);
            newTarget = false;
        }
            */

        return searchPlayer;
    }

    private void IncreasePlayerTarget(string playerName)
    {
        switch (playerName)
        {
            case "Player1":
                enemyDirector.playerTarget[0]++;
                break;
            case "Player2":
                enemyDirector.playerTarget[1]++;
                break;
            case "Player3":
                enemyDirector.playerTarget[2]++;
                break;
            case "Player4":
                enemyDirector.playerTarget[3]++;
                break;
        }
    }

    private void DecreasePlayerTarget(string playerName)
    {
        switch (playerName)
        {
            case "Player1":
                enemyDirector.playerTarget[0]--;
                break;
            case "Player2":
                enemyDirector.playerTarget[1]--;
                break;
            case "Player3":
                enemyDirector.playerTarget[2]--;
                break;
            case "Player4":
                enemyDirector.playerTarget[3]--;
                break;
        }
    }

    private Transform FindSecondClosestPlayer(Transform closestPlayer)
    {
        Transform secondClosestPlayer = null;
        float minDist = float.MaxValue;

        foreach (Transform player in enemyDirector.players)
        {
            if (player != closestPlayer)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance < minDist)
                {
                    minDist = distance;
                    secondClosestPlayer = player;
                }
            }
        }
        return secondClosestPlayer;
    }
}
