using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent : ScriptableObject
{
	public bool eventFinished = false;

	[SerializeField]
	public string eventName = "No name";
	public abstract void EventStart();

	public abstract void EventUpdate();

	public virtual void EventExit()
	{
		eventFinished = true;
	}

}
