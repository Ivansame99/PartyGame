using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    public int[] playerTarget;
    private float currentEnemies;
    public int currentPlayers;
    public float splitEnemies;
    public bool[] full;
    public List<Transform> players;
    public GameObject[] jugadoresArray;
    private RoundController roundController;

    private PlayerHealthController[] playerHealth;
    public int playerDead;

    void Start()
    {
        roundController = this.GetComponent<RoundController>();
        jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            players.Add(jugadorObj.transform);
        }
        playerTarget = new int[jugadoresArray.Length];
        currentEnemies = roundController.currentEnemies.Count;
        currentPlayers = jugadoresArray.Length;
        full = new bool[jugadoresArray.Length];

        playerHealth = new PlayerHealthController[jugadoresArray.Length];

        for (int i = 0; i < currentPlayers; i++)
        {
            if (players[i].GetComponent<PlayerHealthController>() != null)
            {
                playerHealth[i] = players[i].GetComponent<PlayerHealthController>();
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < currentPlayers; i++)
        {
            if (playerHealth[i].dead) playerDead++;
        }

        currentEnemies = roundController.currentEnemies.Count;
        
        splitEnemies = Mathf.Ceil(currentEnemies / (currentPlayers - playerDead));
        
        for (int i = 0; i < currentPlayers; i++)
        {
            if (playerTarget[i] < splitEnemies) full[i] = false;
            else if(playerTarget[i] >= splitEnemies) full[i] = true;
        }

        playerDead = 0;
    }
}
