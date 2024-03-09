using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventsController : MonoBehaviour
{
	[SerializeField]
	private Animator eventsCanvas;

	[SerializeField]
	private TMP_Text eventNameText;

	[SerializeField]
	private float timeToShowUI;

	[SerializeField]
	private List<GameEvent> randomEvents;

	[SerializeField]
	private float maxTimeToSpawn;

	[SerializeField]
	private float minTimeToSpawn;

	private float randomTimeToSpawn;

	private float timer;

	private int eventIndex;

	private bool anim=false;

	private bool onFixedUpdate;

	private bool startUpdate = false;

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
			eventNameText.text = randomEvents[eventIndex].eventName;
			randomEvents[eventIndex].EventStart();
			if (randomEvents[eventIndex].fixedUpdate) onFixedUpdate = true;
			Invoke(nameof(ShowUIEvent), randomTimeToSpawn - timeToShowUI);
		}

		if (timer >= randomTimeToSpawn)
		{
			if (!randomEvents[eventIndex].eventFinished)
			{
				if(anim)
				{
					anim = false;
					eventsCanvas.SetBool("NewEvent", false);
					startUpdate = true;
				}
			}
			else
			{
				startUpdate = false;
				timer = 0;
			}
		} else
		{
			timer += Time.deltaTime;
		}

		if(startUpdate && !randomEvents[eventIndex].fixedUpdate)
		{
			randomEvents[eventIndex].EventUpdate();
		}
	}

	private void FixedUpdate()
	{
		if (startUpdate && randomEvents[eventIndex].fixedUpdate)
		{
			randomEvents[eventIndex].EventUpdate();
		}
	}

	void ShowUIEvent()
	{
		eventsCanvas.SetBool("NewEvent", true);
		anim = true;
	}
}
