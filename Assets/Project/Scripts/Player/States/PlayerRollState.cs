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

	[SerializeField]
	private float inmuneTime;

	[SerializeField]
	private GameObject rollParticle;

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
		player.anim.SetTrigger("Roll");
		player.playerAudioManager.PlayRoll();
		player.isDodging = false;
		player.healthController.invencibleTimer = inmuneTime;
		Instantiate(rollParticle, player.transform.position, rollParticle.transform.rotation);
	}

	public override void Exit()
	{
		player.dodgeTimer = dodgeCD;
		ResetVelocity();
	}

	public override void FixedUpdate()
	{
		Vector3 currentVelocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

		if (currentVelocity.magnitude > dodgeSpeed)
		{
			currentVelocity = currentVelocity.normalized * dodgeSpeed;
		}

		Vector3 targetVelocity = rollDirection * dodgeSpeed;
		Vector3 force = (targetVelocity - currentVelocity) / Time.fixedDeltaTime;
		player.rb.AddForce(force, ForceMode.Acceleration);
	}

	public override void Update()
	{
		if (rollTimer < rollDuration) rollTimer += Time.deltaTime;
		else player.ChangeState(typeof(PlayerIdleState));
	}

	private void ResetVelocity()
	{
		player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
	}
}
