using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateManager<T> : MonoBehaviour where T: MonoBehaviour
{
    public List<PlayerState<T>> states;
    public readonly Dictionary<Type, PlayerState<T>> stateByType = new ();
    private PlayerState<T> currentState;

    protected virtual void Awake()
    {
        states.ForEach(s=> stateByType.Add(s.GetType(), s));
		ChangeState(states[0].GetType());
    }

    public void ChangeState(Type newStateType)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = stateByType[newStateType];
        currentState.Init(GetComponent<T>());

    }

	// Update is called once per frame
	protected virtual void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}
