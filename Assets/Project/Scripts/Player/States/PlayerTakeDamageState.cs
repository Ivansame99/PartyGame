using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/TakeDamege")]
public class PlayerTakeDamageState : PlayerState<PlayerController>
{
	[SerializeField]
	private float stunTime;

	private float timer;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		player.anim.SetTrigger("TakeDamage");
		timer = 0;
		ResetVelocity();
	}

	public override void Update()
	{
		timer += Time.deltaTime;

		if (timer>=stunTime)
		{
			player.ChangeState(typeof(PlayerIdleState));
			return;
		}
	}

	public override void FixedUpdate()
    {

    }

	public override void Exit()
	{
		player.anim.ResetTrigger("TakeDamage");
	}

	private void ResetVelocity()
	{
		player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
	}
}
