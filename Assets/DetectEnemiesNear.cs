using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemiesNear : MonoBehaviour
{
	[HideInInspector]
	public List<GameObject> enemiesNear = new List<GameObject>();

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") || other.CompareTag("Player"))
		{
			enemiesNear.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Enemy") || other.CompareTag("Player"))
		{
			enemiesNear.Remove(other.gameObject);
		}
	}
}
