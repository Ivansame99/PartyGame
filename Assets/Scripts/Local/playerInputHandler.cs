using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;

public class playerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfig;
    private PlayerController playerController;

    private Inputs inputs;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        inputs = new Inputs();
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        playerConfig = pc;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(CallbackContext obj)
    {
        if(obj.action.name == inputs.Player.Movement.name)
        {
            OnMove(obj);
        }
        if (obj.action.name == inputs.Player.Dodge.name && obj.action.WasPressedThisFrame())
        {
            OnDodge(obj);
        }
        if (obj.action.name == inputs.Player.Attack.name && obj.action.WasPressedThisFrame())
        {
            OnAttack(obj);
        }
        if (obj.action.name == inputs.Player.SpecialAttack.name)
        {
            OnSpecialAttack(obj);
        }
		if (obj.action.name == inputs.Player.Jump.name && obj.action.WasPressedThisFrame())
		{
			OnJump(obj);
		}

	}

    public void OnMove(CallbackContext context)
    {
        if(playerController != null)
            playerController.SetInputVector(context.ReadValue<Vector2>());

    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (playerController != null)
            playerController.SetDodge(context.ReadValueAsButton());

    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (playerController != null)
            playerController.SetAttack(context.ReadValueAsButton());
    }
    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        if (playerController != null)
            playerController.SetSpecialAttack(context.ReadValueAsButton());

    }
	public void OnJump(CallbackContext context)
	{
		if (playerController != null)
			playerController.SetJump(context.ReadValueAsButton());

	}

}
