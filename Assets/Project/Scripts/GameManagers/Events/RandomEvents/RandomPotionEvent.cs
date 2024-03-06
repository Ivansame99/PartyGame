using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/RandomPotion")]
public class RandomPotionEvent : GameEvent
{
	[SerializeField]
	private GameObject potionPrefab;

	[SerializeField]
	private float speedMin;

	[SerializeField]
	private float speedMax;

	private float xPosMax = 23f;
	private float xPosMin = -22.5f;
	private float zPosMax = 22f;
	private float zPosMin = -8.5f;
	private float yPos = 50f;

	private Vector3 initialPos;

	bool instancedPotion;
	private GameObject potion;

	float randomXPosInit;
	float randomZPosInit;

	float randomXPosFinal;
	float randomZPosFinal;

	float speed;

	public override void EventStart()
	{
		eventFinished = false;
		instancedPotion = false;
		potion=null;
		randomXPosFinal = Random.Range(xPosMin, xPosMax);
		randomZPosFinal = Random.Range(zPosMin, zPosMax);
		speed = Random.Range(speedMin, speedMax);
	}

	public override void EventUpdate()
	{
		if (!instancedPotion)
		{
			potion = Instantiate(potionPrefab, new Vector3(-33, 16f, 0f), potionPrefab.transform.rotation);
			instancedPotion = true;
		} else
		{
			potion.transform.position = Vector3.MoveTowards(potion.transform.position, new Vector3(randomXPosFinal, 2f, randomZPosFinal), speed * Time.deltaTime);
			if (potion.transform.position == new Vector3(randomXPosFinal, 2f, randomZPosFinal))
			{
				eventFinished = true;
			}
		}
	}
}
