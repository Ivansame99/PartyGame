using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PlayerConfigurationManager : MonoBehaviour
{
    [HideInInspector]
    public List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private GameObject[] playerText;

    [SerializeField]
    private TMP_Text readyPlatformText;

    [SerializeField]
    private GameObject controlsPanel;

    [SerializeField]
    private int maxPlayers;
    private int playerIndex = 0;

    [SerializeField]
    private GameObject[] playerPos;
    [SerializeField]
    private GameObject[] prefabPlayers;

    public int playersReady;

    [SerializeField]
    MultipleTargetCamera targetCamera;

    [HideInInspector]
    public bool onHub = true;

    public static PlayerConfigurationManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public void RumblePulse(int playerIndex, float lowFrequency, float highFrequency, float duration)
    {
        Gamepad pad = playerConfigs[playerIndex].Gamepad;

        if (pad != null)
        {
            pad.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(StopRumbleAfter(duration, pad));
        }
    }

    private IEnumerator StopRumbleAfter(float duration, Gamepad pad)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pad.SetMotorSpeeds(0f, 0f);
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void ReadyPlayer()
    {
        readyPlatformText.text = playersReady.ToString() + " / " + playerConfigs.Count.ToString();
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex) && onHub)
        {
            playerText[playerIndex].GetComponent<Animator>().SetTrigger("Ready");
            controlsPanel.SetActive(true);

            pi.transform.SetParent(transform);

            PlayerConfiguration playerConfig = new PlayerConfiguration(pi);
            playerConfigs.Add(playerConfig);
            SpawnPlayer(playerConfig);
            playerIndex++;
            readyPlatformText.text = playersReady.ToString() + " / " + playerConfigs.Count.ToString();
        }
    }

    private void SpawnPlayer(PlayerConfiguration pc)
    {
        playerPos[pc.PlayerIndex].SetActive(false);
        GameObject player = Instantiate(prefabPlayers[pc.PlayerIndex], playerPos[pc.PlayerIndex].transform.position, playerPos[pc.PlayerIndex].transform.rotation) as GameObject;
        player.name = prefabPlayers[pc.PlayerIndex].name;
        PlayerInputHandler pih = player.GetComponent<PlayerInputHandler>();
        pih.InitializePlayer(pc);
        if (targetCamera != null)
        {
            targetCamera.AddPlayer(player.transform);
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
        Gamepad = (Gamepad)pi.devices.FirstOrDefault(d => d is Gamepad);
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }
    public Gamepad Gamepad { get; set; }
}

