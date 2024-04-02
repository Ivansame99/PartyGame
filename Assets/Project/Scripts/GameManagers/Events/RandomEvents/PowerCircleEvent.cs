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


	public override void EventStart()
	{
		eventFinished = false;
		instantiated = false;
	}

	public override void EventUpdate()
	{
		if (!instantiated)
		{
			circle = Instantiate(circlePrefab, Vector3.zero, circlePrefab.transform.rotation);
			Destroy(circle, circleDuration);
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
