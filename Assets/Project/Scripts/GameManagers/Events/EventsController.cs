using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsController : MonoBehaviour
{
    [SerializeField]
	private List<GameEvent> randomEvents;

	[SerializeField]
	private float maxTimeToSpawn;

	[SerializeField]
	private float minTimeToSpawn;

	private float randomTimeToSpawn;

	private float timer;

	void Start()
    {
		timer = 0;
	}

    void Update()
    {
		if (timer == 0) randomTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);

		timer += Time.deltaTime;

		if (timer >= randomTimeToSpawn)
		{
			int eventIndex = Random.Range(0, randomEvents.Count);
			randomEvents[eventIndex].TriggerEvent();
			timer = 0;
		}
	}
}
