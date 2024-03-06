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
	private float yPos = 3f;
	private float yPosIni = 16f;

	private GameObject potion;
	private Rigidbody potionRb;

	float randomXPosInit;
	float randomZPosInit;

	float randomXPosFinal;
	float randomZPosFinal;

	float speed;

	Vector3 initialPosition;

	public override void EventStart()
	{
		fixedUpdate = true;
		eventFinished = false;
		potion = null;
		initialPosition = new Vector3(-37.5f, yPosIni, 0f);
		randomXPosFinal = Random.Range(xPosMin, xPosMax);
		randomZPosFinal = Random.Range(zPosMin, zPosMax);
		speed = Random.Range(speedMin, speedMax);
	}

	public override void EventUpdate()
	{
		potion = Instantiate(potionPrefab, initialPosition, potionPrefab.transform.rotation);
		potionRb = potion.GetComponent<Rigidbody>();
		Vector3 direction = (new Vector3(randomXPosFinal, yPos, randomZPosFinal) - potion.transform.position).normalized;

		potionRb.AddForce(direction * speed, ForceMode.Impulse);
		eventFinished = true;
	}
}
