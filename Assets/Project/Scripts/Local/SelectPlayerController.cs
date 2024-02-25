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

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;

        try
        {
            var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
            for (int i = 0; i < playerConfigs.Length; i++)
            {
                playersUI[i].SetActive(true);
                playerPos[i].SetActive(false);
                GameObject player = Instantiate(prefabPlayers[i], playerPos[i].transform.position, playerPos[i].transform.rotation) as GameObject;
                player.name = prefabPlayers[i].name;
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
            }
        }
        catch (Exception)
        {
            SceneManager.LoadScene("HUB");
		}   
    }
}
