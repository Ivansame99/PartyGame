using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameEnums;

public class ReadyPlatformController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfigurationManager playerConfigurationManager;

	[SerializeField]
	private float timeToChangeScene = 2.0f;

	[SerializeField]
	private ArenaSelector arenaSelector;

	[SerializeField]
	private GameObject hubUI;

	private float changeSceneTimer;

	private bool selectingArena = false;
	private void Start()
	{
		changeSceneTimer = timeToChangeScene;
	}

	private void Update()
	{
		if (playerConfigurationManager.playerConfigs.Count == 0) return;

		if (playerConfigurationManager.playerConfigs.Count == playerConfigurationManager.playersReady)
		{
			changeSceneTimer -= Time.deltaTime;

		}
		else changeSceneTimer = timeToChangeScene;

		if (changeSceneTimer <= 0 && !selectingArena)
		{
			playerConfigurationManager.onHub = false;
			//GameManager.Instance.gmSceneManager.ChangeSceneToArena1(true);
			hubUI.SetActive(false);
			arenaSelector.Scroll();
			selectingArena = true;
		} else
		{
			if (arenaSelector.GetSelectedArena() != GameEnums.Arenas.None)
			{
				GameEnums.Arenas arena = arenaSelector.GetSelectedArena();

				switch (arena)
				{
					case Arenas.StandardArena:
						Debug.Log("Arena estándar");
						GameManager.Instance.gmSceneManager.ChangeSceneToArena1(true);
						break;
					case Arenas.SnowArena:
						Debug.Log("Arena de nieve");
						GameManager.Instance.gmSceneManager.ChangeSceneToArena1(true);
						break;
					default:
						Debug.Log("Arena no reconocida");
						GameManager.Instance.gmSceneManager.ChangeSceneToArena1(true);
						break;
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) playerConfigurationManager.playersReady++;
		playerConfigurationManager.ReadyPlayer();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player")) playerConfigurationManager.playersReady--;
		playerConfigurationManager.ReadyPlayer();
	}
}
