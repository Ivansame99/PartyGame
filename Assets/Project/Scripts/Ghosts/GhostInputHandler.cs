using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GhostInputHandler : MonoBehaviour
{
	private PlayerConfiguration playerConfig;
	private GhostController ghostController;

	private Inputs inputs;
	// Start is called before the first frame update
	void Awake()
	{
		ghostController = GetComponent<GhostController>();
		inputs = new Inputs();
	}

	public void InitializeGhost(PlayerConfiguration pc)
	{
		playerConfig = pc;
		playerConfig.Input.onActionTriggered += Input_onActionTriggered;
	}

	private void Input_onActionTriggered(CallbackContext obj)
	{
		if (obj.action.name == inputs.Player.Movement.name)
		{
			OnMove(obj);
		}
	}

	public void OnMove(CallbackContext context)
	{
		if (ghostController != null)
			ghostController.SetInputVector(context.ReadValue<Vector2>());
	}
}
