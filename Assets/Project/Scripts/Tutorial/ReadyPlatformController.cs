using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyPlatformController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfigurationManager playerConfigurationManager;

	[SerializeField]
	private float timeToChangeScene = 2.0f;

	[Header("Level names")]
	[SerializeField]
	private string levelName;

	private float changeSceneTimer;

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

		if (changeSceneTimer <= 0)
		{
			ChangeScene();
		}
	}

	void ChangeScene()
	{
		SceneManager.LoadScene(levelName);
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
