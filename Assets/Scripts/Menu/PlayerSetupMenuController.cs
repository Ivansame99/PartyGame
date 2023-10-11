using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject hatPanel;

    private float ignoreInputTime = 1.5f;
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
        readyPanel.SetActive(true);
        hatPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        hatPanel.SetActive(false);
        readyPanel.SetActive(true);
    }
}
