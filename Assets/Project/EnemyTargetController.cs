using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetController : MonoBehaviour
{
    private EnemyDirector enemyDirector;

    //Targets
    private bool newTarget;
    private Transform lastTarget;

    [HideInInspector]
    public Transform player, player2;

    private Enemy enemy;

    void Start()
    {
        enemyDirector = GameObject.Find("GameManager").GetComponent<EnemyDirector>();
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy.playerPos = FindPlayer();
        enemy.playerPos2 = FindSecondClosestPlayer(player);
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
        if (!newTarget && searchPlayer != null)
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

    public void DecreasePlayerTarget(string playerName)
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
