using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using System.Linq;

public class SelectPlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    public GameObject player1Pos;
    [SerializeField]
    public GameObject player2Pos;
    [SerializeField]
    public GameObject prefabPlayer,prefabPlayer2;
    [SerializeField]
    public GameObject character;

    // Start is called before the first frame update
    void Awake()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            if (i == 0)
            {
                
                player1Pos.SetActive(false);
                GameObject player1 = Instantiate(prefabPlayer, player1Pos.transform.position, player1Pos.transform.rotation) as GameObject;

                //player1.transform.parent = character.transform;
                Debug.Log("player2");
                player1.GetComponent<playerInputHandler>().InitializePlayer(playerConfigs[i]);
                Debug.Log("player3");
            }
            if (i == 1)
            {
                
                    player2Pos.SetActive(false);
                    GameObject player2 = Instantiate(prefabPlayer2, player2Pos.transform.position, player2Pos.transform.rotation) as GameObject;
                    //player2.transform.parent = character.transform;
                    player2.GetComponent<playerInputHandler>().InitializePlayer(playerConfigs[i]);
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
