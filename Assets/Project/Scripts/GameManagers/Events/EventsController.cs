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
			eventNameText.text = randomEvents[0].eventName;
			randomEvents[0].EventStart();
			if (randomEvents[0].fixedUpdate) onFixedUpdate = true;
			Invoke(nameof(ShowUIEvent), randomTimeToSpawn - timeToShowUI);
		}

		if (timer >= randomTimeToSpawn)
		{
			if (!randomEvents[0].eventFinished)
			{
				if(anim)
				{
					anim = false;
					eventsCanvas.SetBool("NewEvent", false);
				}
				randomEvents[0].EventUpdate();
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

	private void FixedUpdate()
	{
		if()
	}

	void ShowUIEvent()
	{
		eventsCanvas.SetBool("NewEvent", true);
		anim = true;
	}
}
