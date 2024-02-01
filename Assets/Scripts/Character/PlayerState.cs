using UnityEngine;

public abstract class PlayerState<T> : ScriptableObject where T : MonoBehaviour
{
    protected T player;

    public virtual void Init(T p)
    {
        player = p;
    }

	public abstract void Start();

	public abstract void CaptureInput();

	public abstract void Update();

	public abstract void FixedUpdate();

	public abstract void ChangeState();

	public abstract void Exit();
}
