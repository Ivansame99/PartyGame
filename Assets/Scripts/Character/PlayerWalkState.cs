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

	private Rigidbody rb;
	private GroundCheck groundCheck;
	private Animator anim;

	private float turnSmoothTime;

	private Vector3 direction;

	private float runCounter = 0;
	private float runCounterRandom;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		if (rb == null) rb = p.GetComponent<Rigidbody>();
		if (groundCheck == null) groundCheck = p.GetComponent<GroundCheck>();
		if (anim == null) anim = p.GetComponent<Animator>();

	}

	public override void Exit()
	{
		anim.SetBool("Walking", false);
	}

	public override void FixedUpdate()
	{
		rb.MovePosition(player.transform.position + direction * speed * Time.fixedDeltaTime);
	}

	public override void Update()
	{
		direction = new Vector3(player.moveUniversal.x, 0f, player.moveUniversal.y).normalized;
		//Change to Idle
		if (direction.magnitude < 0.1f)
		{
			player.ChangeState(typeof(PlayerIdleState));
			return;
		}

		//Walk
		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
		player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

		runCounter += Time.deltaTime;
		if (runCounter >= runCounterRandom)
		{
			Instantiate(runParticles, new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z - 1), Quaternion.identity);
			runCounter = 0;
			runCounterRandom = Random.Range(0.1f, 0.5f);
		}

		anim.SetBool("Walking", true);
	}
}
