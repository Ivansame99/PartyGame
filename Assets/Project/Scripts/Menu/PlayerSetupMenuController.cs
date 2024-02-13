using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField]
    private GameObject waitingPanel;
    [SerializeField]
    private Button readyButton;

    private float ignoreInputTime = 0.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        ignoreInputTime = Time.deltaTime + ignoreInputTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetPrefab(int hat)
    {
        if (!inputEnabled) { return; }
        readyButton.gameObject.SetActive(true);
        readyButton.Select();
        //EventSystem.current.SetSelectedGameObject(this.gameObject);
        //hatPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }
        //PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        //hatPanel.SetActive(false);
        readyButton.gameObject.SetActive(false);
        waitingPanel.SetActive(true);
        SceneManager.LoadScene("Enemy2");
    }
}
