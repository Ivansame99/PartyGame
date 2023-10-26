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
    private Character2Controller playerController;

    private Inputs inputs;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<Character2Controller>();
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
        if (obj.action.name == inputs.Player.Dodge.name)
        {
            OnDodge(obj);
        }
        if (obj.action.name == inputs.Player.Attack.name)
        {
            OnAttack(obj);
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

}
