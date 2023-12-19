using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SelectPlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject[] playerPos;
    [SerializeField]
    public GameObject[] prefabPlayers;
    [SerializeField]
    public GameObject[] playersUI;
    [SerializeField]
    public GameObject character;

    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
            for (int i = 0; i < playerConfigs.Length; i++)
            {
                playersUI[i].SetActive(true);
                playerPos[i].SetActive(false);
                GameObject player = Instantiate(prefabPlayers[i], playerPos[i].transform.position, playerPos[i].transform.rotation) as GameObject;
                player.name = prefabPlayers[i].name;
                //player1.transform.parent = character.transform;
                player.GetComponent<playerInputHandler>().InitializePlayer(playerConfigs[i]);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Te has olvidado de inciar el juego en la escena PlayerJoin");
            //  Block of code to handle errors
            //GameObject player = Instantiate(prefabPlayer, playerPos[0].transform.position, playerPos[0].transform.rotation) as GameObject;
            //player1.transform.parent = character.transform;
            //player.GetComponent<playerInputHandler>().InitializePlayer(playerConfigs[0]);
        }   
    }
}
