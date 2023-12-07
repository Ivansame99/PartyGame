using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersRespawn : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawns;

    private RoundController roundController;
    // Start is called before the first frame update
    void Start()
    {
        roundController = GetComponent<RoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer(GameObject player)
    {
        if (!roundController.finalRound)
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
}
