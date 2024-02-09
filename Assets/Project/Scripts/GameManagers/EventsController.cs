using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsController : MonoBehaviour
{
	public RandomPotion randomPotionEvent;

	void Start()
    {
        
    }

    void Update()
    {
        
    }

    [System.Serializable]
    public class RandomPotion
    {
		[SerializeField]
		private GameObject potionPrefab;

		[SerializeField]
		private float maxTimeToSpawn;

		[SerializeField]
		private float minTimeToSpawn;

		private float randomTimeToSpawn;

		private float xPosMax = 18f;
		private float xPosMin = -19f;
		private float zPosMax = 19f;
		private float zPosMin = -9f;
		private float yPos = 50f;

		private float timer;

	}
}
