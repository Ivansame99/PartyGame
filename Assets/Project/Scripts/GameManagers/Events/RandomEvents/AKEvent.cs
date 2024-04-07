using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Ak")]
public class AKEvent : GameEvent
{
	[SerializeField]
	private float eventDuration;

	private float timer;
	private bool setAk = false;
	private PlayerController[] playerController;
	public override void EventStart()
	{
		eventFinished = false;
		setAk = false;
		timer = 0;
		playerController = GameManager.Instance.selectPlayerController.GetPlayersController();
	}

	public override void EventUpdate()
	{
		if (!setAk)
		{
			//int randomPlayer = Random.Range(0, GameManager.Instance.selectPlayerController.GetNumPlayers());

			//playerController[randomPlayer].ak = true;

			for (int i = 0; i < playerController.Length; i++)
			{
				playerController[i].ak = true;
			}

			setAk = true;
		}

		if (timer >= eventDuration)
		{
			for (int i = 0; i < playerController.Length; i++)
			{
				playerController[i].ak = false;
			}
			eventFinished = true;
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	public override void EventDestroy()
	{
		for (int i = 0; i < playerController.Length; i++)
		{
			playerController[i].ak = false;
		}
		eventFinished = true;
	}
}
