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
	private float timeBetweenPreviewAndAttack;

	private List<Vector3> thunderPos = new List<Vector3>();

	private float xPosMax = 18f;
	private float xPosMin = -19f;
	private float zPosMax = 19f;
	private float zPosMin = -9f;
	private float previewYPos = 0.9f;
	private float attackYPos = 10f;

	private bool allThundersFalled = false;

	private float timer;

	private bool preview = false;
	public override void EventStart()
	{
		thunderPos.Clear();
		eventFinished = false;
		allThundersFalled = false;
		preview = false;
		timer = 0;
	}

	public override void EventUpdate()
    {
		if (!eventFinished)
		{
			if (!preview)
			{
				for (int i = 0; i < thundersNumber; i++)
				{
					float randomXPos = Random.Range(xPosMin, xPosMax);
					float randomZPos = Random.Range(zPosMin, zPosMax);

					thunderPos.Add(new Vector3(randomXPos, previewYPos, randomZPos));

					Instantiate(previewAttack, thunderPos[i], previewAttack.transform.rotation);
				}
				preview = true;
			}
			else
			{
				if(timer>= timeBetweenPreviewAndAttack)
				{
					for (int i = 0; i < thundersNumber; i++)
					{
						Vector3 pos = new Vector3(thunderPos[i].x, attackYPos, thunderPos[i].z);
						//thunderPos[i].y = attackYPos;
						Destroy(Instantiate(thunderAttack, pos, thunderAttack.transform.rotation), 1f);
					}

					eventFinished = true;
				} else timer += Time.deltaTime;
			}	
		}
	}
}
