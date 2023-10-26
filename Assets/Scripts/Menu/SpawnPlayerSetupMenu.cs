using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    [Header("Logic select players Variables")]
    [SerializeField]
    public GameObject playerSetupMenuPrefab,playerSetupMenuPrefab2;
    [SerializeField]
    public PlayerInput input;


    private void Awake()
    {
        var rootMenu = GameObject.Find("PlayerCanvas3");
        if(rootMenu != null)
        {

            if (input.playerIndex == 0)
            {
                var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);
                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
                PlayerConfigurationManager.Instance.ReadyPlayer(0);
            }
            if (input.playerIndex == 1)
            {
                var menu = Instantiate(playerSetupMenuPrefab2, rootMenu.transform);
                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
                PlayerConfigurationManager.Instance.ReadyPlayer(1);
            }

        }
    }
}
