using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Roll")]
public class PlayerRollState : PlayerState<PlayerController>
{
	[SerializeField]
	private float dodgeSpeed;

	[SerializeField]
	private float dodgeCD;

	[SerializeField]
	private float turnSmooth;

	[SerializeField]
	private float rollDuration;

	private Vector3 rollDirection;
	private float turnSmoothTime;

	private float rollTimer = 0;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		
		if (player.direction == Vector3.zero)
		{
			rollDirection = player.transform.forward;
		}
		else
		{
			rollDirection = player.direction;
			float targetAngle = Mathf.Atan2(player.direction.x, player.direction.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
			player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}

		rollTimer = 0;
		player.dodgeTimer = dodgeCD;
		player.anim.SetTrigger("Roll");
		player.dodgeSound.Play();
		player.isDodging = false;
	}

	public override void Exit()
	{

	}

	public override void FixedUpdate()
	{
		player.rb.MovePosition(player.transform.position + rollDirection * dodgeSpeed * Time.fixedDeltaTime);
	}

	public override void Update()
	{
		if (rollTimer < rollDuration) rollTimer += Time.deltaTime;
		else player.ChangeState(typeof(PlayerIdleState));
	}
}
