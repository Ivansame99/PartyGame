using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyDirector : MonoBehaviour
{
    public int[] playerTarget;
    private int currentEnemies;
    private int currentPlayers;
    public int splitEnemies;
    public List<Transform> players;
    public GameObject[] jugadoresArray;
    private RoundController roundController;
    // Start is called before the first frame update
    void Start()
    {
        roundController = this.GetComponent<RoundController>();
        currentEnemies = roundController.currentEnemies.Count;
        currentPlayers = roundController.playersCount;
        jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            players.Add(jugadorObj.transform);
        }
        playerTarget = new int[jugadoresArray.Length];
    }

    // Update is called once per frame
    void Update()
    {
        currentEnemies = roundController.currentEnemies.Count;
        currentPlayers = roundController.playersCount;
        splitEnemies = currentEnemies / currentPlayers;

        /*
        for(int i = 0; i < playerTarget.Length; i++)
        {
            if (playerTarget[i] > splitEnemies)
            {

            }
        }
        */
        Debug.Log(playerTarget[0]);
        Debug.Log(playerTarget[1]);
    }
}
