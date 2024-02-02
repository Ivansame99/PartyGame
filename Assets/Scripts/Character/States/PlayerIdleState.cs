using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Idle")]
public class PlayerIdleState : PlayerState<PlayerController>
{
	public override void Init(PlayerController p)
	{
		base.Init(p);
	}

	public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
		//Change to walk
		if (player.direction.magnitude >= 0.1f)
		{
			player.ChangeState(typeof(PlayerWalkState));
			return;
		}

		//Change to roll
		if (player.isDodging && player.dodgeTimer <= 0)
		{
			player.ChangeState(typeof(PlayerRollState));
			return;
		}

		//Change to jump
		if (player.groundCheck.DetectGround() && player.isJumping)
		{
			player.ChangeState(typeof(PlayerJumpState));
			return;
		}
	}
}
