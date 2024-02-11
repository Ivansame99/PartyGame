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

	private int eventIndex;

	void Start()
    {
		timer = 0;
	}

    void Update()
    {
		if (timer == 0)
		{
			eventIndex = Random.Range(0, randomEvents.Count);
			randomTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
			randomEvents[1].EventStart();
		}

		if (timer >= randomTimeToSpawn)
		{
			if (!randomEvents[1].eventFinished)
			{
				randomEvents[1].EventUpdate();
			}
			else
			{
				timer = 0;
			}
		} else
		{
			timer += Time.deltaTime;
		}
	}
}
