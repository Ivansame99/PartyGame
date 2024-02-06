using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyDirector : MonoBehaviour
{
    public int[] playerTarget;
    private float currentEnemies;
    public float currentPlayers;
    public float splitEnemies;
    public bool[] full;
    public List<Transform> players;
    public GameObject[] jugadoresArray;
    private RoundController roundController;

    private PlayerHealthController[] playerHealth;
    public int playerDead;
    // Start is called before the first frame update
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

    // Update is called once per frame
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
