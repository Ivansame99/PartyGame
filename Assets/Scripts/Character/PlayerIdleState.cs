using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Idle")]
public class PlayerIdleState : PlayerState<PlayerController>
{
	private Vector3 direction;

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
		direction = new Vector3(player.moveUniversal.x, 0f, player.moveUniversal.y).normalized;
		if (direction.magnitude >= 0.1f)
		{
			player.ChangeState(typeof(PlayerWalkState));
		}
	}
}
