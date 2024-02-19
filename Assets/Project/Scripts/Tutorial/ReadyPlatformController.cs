using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyPlatformController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfigurationManager playerConfigurationManager;

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
