using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "Events/RandomPotion")]
public class RandomPotionEvent : GameEvent
{
	[SerializeField]
	private GameObject[] objectThrowed;

	private GameObject currentObject;

	[SerializeField]
	private float speedMin;

	[SerializeField]
	private float speedMax;

	[SerializeField]
	private float destroyTime;

	private float xPosMaxIni = 37f;
	private float xPosMinIni = -37f;

	private float zPosMaxIni = 27f;
	private float zPosMinIni = -5f;
	private float yPosIni = 20;

	private float xPosMaxFinal = 23f;
	private float xPosMinFinal = -22.5f;
	private float zPosMaxFinal = 22f;
	private float zPosMinFinal = -8.5f;
	private float yPosFinal = 10;

	private GameObject potion;
	private Rigidbody potionRb;

	float randomZPosIni;

	float randomXPosFinal;
	float randomZPosFinal;

	float speed;

	Vector3 initialPosition;
	Vector3 finalPosition;

	[FMODUnity.EventRef] 
    public string waterEventPath = "event:/SFX/UI/Accept"; //cambiar evento

	public override void EventStart()
	{
		int randomObjectIndex = Random.Range(0, objectThrowed.Length);
		currentObject = objectThrowed[randomObjectIndex];

		xPosMaxIni = 37f;
		xPosMinIni = -37f;

		zPosMaxIni = 27f;
		zPosMinIni = -10f;
		yPosIni = 20;

		xPosMaxFinal = 23f;
		xPosMinFinal = -22.5f;
		zPosMaxFinal = 22f;
		zPosMinFinal = -8.5f;
		yPosFinal = 10;

		fixedUpdate = true;
		eventFinished = false;
		potion = null;
		randomZPosIni = Random.Range(zPosMinIni, zPosMaxIni);
		if (Random.Range(0, 2) == 1)
		{
			initialPosition = new Vector3(xPosMinIni, yPosIni, randomZPosIni);
		}
		else
		{
			initialPosition = new Vector3(xPosMaxIni, yPosIni, randomZPosIni);
		}
		randomXPosFinal = Random.Range(xPosMinFinal, xPosMaxFinal);
		randomZPosFinal = Random.Range(zPosMinFinal, zPosMaxFinal);
		finalPosition = new Vector3(randomXPosFinal, yPosFinal, Random.Range(zPosMinFinal, zPosMaxFinal));
		speed = Random.Range(speedMin, speedMax);
	}

	public override void EventUpdate()
	{
		potion = Instantiate(currentObject, initialPosition, currentObject.transform.rotation);
		FMODUnity.RuntimeManager.PlayOneShot(waterEventPath);
		Destroy(potion, destroyTime);
		potionRb = potion.GetComponent<Rigidbody>();
		float distanceToTarget = Vector3.Distance(potion.transform.position, finalPosition);
		float initialSpeed = Mathf.Sqrt(2 * distanceToTarget * speed);

		Vector3 direction = (finalPosition - potion.transform.position).normalized;
		potionRb.velocity = direction * initialSpeed;
		eventFinished = true;
	}

	public override void EventDestroy()
	{
		eventFinished = true;
		if(potion!=null) Destroy(potion);
	}
}
