using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Ak")]
public class AKEvent : GameEvent
{
	[SerializeField]
	private float eventDuration;

	private float timer;
	private bool setAk=false;
	private PlayerController[] playerController;
	public override void EventStart()
    {
		eventFinished = false;
		setAk = false;
		timer = 0;
		if (playerController == null) playerController = GameManager.Instance.selectPlayerController.GetPlayersController();
	}

    public override void EventUpdate()
    {
		if (!setAk)
		{
			for(int i = 0; i < playerController.Length; i++)
			{
				playerController[i].ak = true;
			}
			setAk=true;
		}

		if (timer >= eventDuration)
		{
			for (int i = 0; i < playerController.Length; i++)
			{
				playerController[i].ak = false;
			}
			eventFinished = true;
		} else
		{
			timer +=Time.deltaTime;
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
