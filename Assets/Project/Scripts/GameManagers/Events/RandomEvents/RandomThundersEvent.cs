using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/RandomThunders")]
public class RandomThundersEvent : GameEvent
{
	[SerializeField]
	private GameObject previewAttack;

	[SerializeField]
	private GameObject thunderAttack;

	[SerializeField]
	private int thundersNumber;

	[SerializeField]
	private float minPreViewDelay;

	[SerializeField]
	private float maxPreViewDelay;

	[SerializeField]
	private float minAttackDelay;

	[SerializeField]
	private float maxAttackDelay;

	private float xPosMax = 18f;
	private float xPosMin = -19f;
	private float zPosMax = 19f;
	private float zPosMin = -9f;
	private float previewYPos = 0.9f;
	private float attackYPos = 10f;

	private CoroutineManager coroutineManager;

	public override void EventStart()
	{
		eventFinished = false;
		if (coroutineManager == null) coroutineManager = CoroutineManager.Instance;
	}

	public override void EventUpdate()
	{
		if (!eventFinished)
		{
			for (int i = 0; i < thundersNumber; i++)
			{
				float preViewDelay = Random.Range(minPreViewDelay, maxPreViewDelay);

				float randomXPos = Random.Range(xPosMin, xPosMax);
				float randomZPos = Random.Range(zPosMin, zPosMax);

				Vector3 position = new Vector3(randomXPos, previewYPos, randomZPos);

				float attackDelay = Random.Range(minAttackDelay, maxAttackDelay);

				coroutineManager.StartCoroutineFromScriptableObject(SpawnThunder(preViewDelay, position, attackDelay));
			}
			eventFinished = true;
		}
	}

	private IEnumerator SpawnThunder(float preViewDelay, Vector3 pos, float attackDelay)
	{
		yield return new WaitForSeconds(preViewDelay);
		Instantiate(previewAttack, pos, previewAttack.transform.rotation);
		yield return new WaitForSeconds(attackDelay);
		Vector3 attackPos = new Vector3(pos.x, attackYPos, pos.z);
		Destroy(Instantiate(thunderAttack, attackPos, thunderAttack.transform.rotation), 0.5f);
	}
}
