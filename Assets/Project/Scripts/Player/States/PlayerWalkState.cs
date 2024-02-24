using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Walk")]

public class PlayerWalkState : PlayerState<PlayerController>
{
	[SerializeField]
	private float speed;

	[SerializeField]
	private float turnSmooth;

	[SerializeField]
	private GameObject runParticles;

	private float turnSmoothTime;

	private float runCounter = 0;
	private float runCounterRandom;

	public override void Init(PlayerController p)
	{
		base.Init(p);
	}

	public override void Exit()
	{
		player.anim.SetBool("Walking", false);
		ResetVelocity();
	}

	public override void FixedUpdate()
	{
		Vector3 currentVelocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

		if (currentVelocity.magnitude > speed)
		{
			currentVelocity = currentVelocity.normalized * speed;
		}

		Vector3 targetVelocity = player.direction * speed;
		Vector3 force = (targetVelocity - currentVelocity) / Time.fixedDeltaTime;
		player.rb.AddForce(force, ForceMode.Acceleration);
	}

	public override void Update()
	{
		//Change to Idle
		if (player.direction.magnitude < 0.1f)
		{
			player.ChangeState(typeof(PlayerIdleState));
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

		float targetAngle = Mathf.Atan2(player.direction.x, player.direction.z) * Mathf.Rad2Deg;
		float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
		if (!float.IsNaN(angle))
		{
			var rotation = Quaternion.Euler(0f, angle, 0f);
			player.transform.rotation = rotation;
		}

		if (player.groundCheck.DetectGround())
		{
			runCounter += Time.deltaTime;
			if (runCounter >= runCounterRandom)
			{
				Instantiate(runParticles, new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z - 1), Quaternion.identity);
				runCounter = 0;
				runCounterRandom = Random.Range(0.1f, 0.5f);
			}
		}

		player.anim.SetBool("Walking", true);
	}

	private void ResetVelocity()
	{
		player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
	}
}
