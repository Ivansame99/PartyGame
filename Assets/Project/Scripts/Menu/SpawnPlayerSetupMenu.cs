using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    [Header("Logic select players Variables")]
    [SerializeField]
    public GameObject[] playerSetupMenuPrefab;
    [SerializeField]
    public PlayerInput input;
    private int maxPlayers = 4;


    private void Awake()
    {
        var rootMenu = GameObject.Find("PlayerCanvas3");
        if(rootMenu != null)
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                if (input.playerIndex == i)
                {
                    var menu = Instantiate(playerSetupMenuPrefab[i], rootMenu.transform);
                    input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                    menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(i);
                    PlayerConfigurationManager.Instance.ReadyPlayer(i);
                }
            }
        }
    }
}
