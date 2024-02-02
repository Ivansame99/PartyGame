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
	}

	public override void FixedUpdate()
	{
		player.rb.MovePosition(player.transform.position + player.direction * speed * Time.fixedDeltaTime);
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
		if (player.isDodging && player.dodgeTimer<=0)
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

		//Walk
		float targetAngle = Mathf.Atan2(player.direction.x, player.direction.z) * Mathf.Rad2Deg;
		float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
		player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

		runCounter += Time.deltaTime;
		if (runCounter >= runCounterRandom)
		{
			Instantiate(runParticles, new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z - 1), Quaternion.identity);
			runCounter = 0;
			runCounterRandom = Random.Range(0.1f, 0.5f);
		}

		player.anim.SetBool("Walking", true);
	}
}
