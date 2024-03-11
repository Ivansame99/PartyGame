using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent : ScriptableObject
{
	internal bool eventFinished = false;
	internal bool fixedUpdate = false;

	[SerializeField]
	public string eventName = "No name";
	public abstract void EventStart();

	public abstract void EventUpdate();

	public abstract void EventDestroy();

	public virtual void EventExit()
	{
		eventFinished = true;
	}

}
