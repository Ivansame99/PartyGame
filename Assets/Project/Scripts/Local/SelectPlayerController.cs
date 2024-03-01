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
    MultipleTargetCamera targetCamera;

    [HideInInspector]
    public int numPlayers;
	// Start is called before the first frame update
	void Awake()
    {

        targetCamera = this.GetComponent<MultipleTargetCamera>();
        Time.timeScale = 1f;

        try
        {
            var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
            numPlayers = playerConfigs.Length;
            for (int i = 0; i < playerConfigs.Length; i++)
            {
                playersUI[i].SetActive(true);
                playerPos[i].SetActive(false);
                GameObject player = Instantiate(prefabPlayers[i], playerPos[i].transform.position, playerPos[i].transform.rotation) as GameObject;
                player.name = prefabPlayers[i].name;
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
				if (targetCamera != null)
                {
                    targetCamera.AddPlayer(player.transform);
                }

			}
        }
        catch (Exception)
        {
            SceneManager.LoadScene("HUB");
		}   
    }
}
