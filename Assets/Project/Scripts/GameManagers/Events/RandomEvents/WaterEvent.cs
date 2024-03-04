using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Events/Water")]
public class WaterEvent : GameEvent
{
	[SerializeField]
	private GameObject waterPrefab;
	[SerializeField]
	private Vector3 startPosition;
	[SerializeField]
	private float maxScale;
	[SerializeField]
	private float yScale;
	[SerializeField]
	private float waterExpandingSpeed;

	[SerializeField]
	private float waterDuration;

	private CoroutineManager coroutineManager;

	private bool waterExpanding;
	private bool waterRetract;

	private GameObject water;

	private float timer;

	private float waterTimer;

	public override void EventStart()
    {
		eventFinished = false;
		if (coroutineManager == null) coroutineManager = CoroutineManager.Instance;
		water = Instantiate(waterPrefab, startPosition, Quaternion.identity);
		water.transform.localScale = Vector3.zero;
		timer = 0;
		waterTimer = 0;
		waterExpanding=true;
		waterRetract = false;
	}

    public override void EventUpdate()
    {
		if (waterExpanding)
		{
			water.transform.localScale = new Vector3(timer, yScale, timer);
			timer += waterExpandingSpeed * Time.deltaTime;
			if(timer >= maxScale) waterExpanding=false;
		}

		if (waterTimer >= waterDuration)
		{
			waterRetract = true;
		} else if(!waterExpanding && !waterRetract)
		{
			waterTimer+=Time.deltaTime;
		}

		if (waterRetract)
		{
			water.transform.localScale = new Vector3(timer, yScale, timer);
			timer -= waterExpandingSpeed * Time.deltaTime;
			if (timer <= 0)
			{
				Destroy(water);
				eventFinished = true;
			}
		}
	}
}
