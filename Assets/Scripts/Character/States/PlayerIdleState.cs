using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Idle")]
public class PlayerIdleState : PlayerState<PlayerController>
{
	[SerializeField]
	private float turnSmooth;

	private float turnSmoothTime;

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
		if (player.isJumping && player.groundCheck.DetectGround())
		{
			player.ChangeState(typeof(PlayerJumpState));
			return;
		}

		//Change to drop attack
		if (player.isJumping && !player.groundCheck.DetectGround())
		{
			player.ChangeState(typeof(PlayerDropAttackState));
			return;
		}

		//Change to attack
		if (player.attackBuffer.Count >= 1 && player.lastComboTimer <= 0)
		{
			player.ChangeState(typeof(PlayerAttackState));
			return;
		}

		//Change to shoot arrow
		if (player.isSpecialAttacking && player.bowTimer <= 0)
		{
			player.ChangeState(typeof(PlayerArrowState));
			return;
		}
	}
}
