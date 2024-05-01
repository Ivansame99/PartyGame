using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/PowerCircle")]
public class PowerCircleEvent : GameEvent
{
	[SerializeField]
	private GameObject circlePrefab;

	[SerializeField]
	private float circleDuration;

	private bool instantiated = false;
	private GameObject circle;

	private Vector3 spawnZone= new Vector3 (0,2,0);

	public override void EventStart()
	{
		eventFinished = false;
		instantiated = false;
	}

	public override void EventUpdate()
	{
		if (!instantiated)
		{
			circle = Instantiate(circlePrefab, spawnZone, circlePrefab.transform.rotation);
			Destroy(circle, circleDuration);
			instantiated = true;
		}

		if (circle == null)
		{
			eventFinished = true;
		}
	}

	public override void EventDestroy()
	{
		Destroy(circle);
		eventFinished = true;
	}
}
