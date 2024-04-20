using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventsController : MonoBehaviour
{
	#region Inspector Variables
	[SerializeField]
	private Animator eventsCanvas;

	[SerializeField]
	private TMP_Text eventNameText;

	[SerializeField]
	private float timeToShowUI;

	[System.Serializable]
	public struct RandomEvents
	{
		public GameEvent randomEvent;
		[Range(0, 1)]
		public float probability;
	}

	[SerializeField]
	private List<RandomEvents> randomEvents;

	[SerializeField]
	private float maxTimeToSpawn;

	[SerializeField]
	private float minTimeToSpawn;
	#endregion

	#region Variables
	private GameEvent currentEvent;

	private float randomTimeToSpawn;

	private float timer;

	private bool anim=false;

	private bool startUpdate = false;

	private bool stopEvents = false;
	#endregion

	#region Life Cycle
	void Start()
    {
		timer = 0;
	}

    void Update()
    {
		if (randomEvents == null || randomEvents.Count == 0 || stopEvents)
		{
			return;
		}

		if (timer == 0)
		{
			SelectNewEvent();
			randomTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
			eventNameText.text = currentEvent.eventName;
			currentEvent.EventStart();
			Invoke(nameof(ShowUIEvent), randomTimeToSpawn - timeToShowUI);
		}

		if (timer >= randomTimeToSpawn)
		{
			if (!currentEvent.eventFinished)
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

		if(startUpdate && !currentEvent.fixedUpdate)
		{
			currentEvent.EventUpdate();
		}
	}

	private void FixedUpdate()
	{
		if (startUpdate && currentEvent.fixedUpdate)
		{
			currentEvent.EventUpdate();
		}
	}
	#endregion

	#region Public Methods
	public void StopEvents()
	{
		stopEvents = true;
		eventsCanvas.gameObject.SetActive(false);
		currentEvent.EventDestroy();
	}
	#endregion

	#region Private Methods
	private void SelectNewEvent()
	{
		if (randomEvents.Count == 0)
		{
			Debug.LogWarning("No hay eventos configurados.");
			return;
		}

		float totalProbability = 0f;
		foreach (var ev in randomEvents)
		{
			totalProbability += ev.probability;
		}

		float randomValue = Random.Range(0f, totalProbability);

		foreach (var ev in randomEvents)
		{
			randomValue -= ev.probability;
			if (randomValue <= 0)
			{
				currentEvent = ev.randomEvent;
				return;
			}
		}
	}

	void ShowUIEvent()
	{
		eventsCanvas.SetBool("NewEvent", true);
		anim = true;
	}
	#endregion

}
