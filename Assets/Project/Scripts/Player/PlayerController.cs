using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : PlayerStateManager<PlayerController>
{
	//Roll
	[HideInInspector]
	public float dodgeTimer = 0;

	//Audio
	[Header("Audio")]
	public AudioSource swordAttackSound;
	public AudioSource bowAttackSound;
	public AudioSource dodgeSound;
	public AudioSource tensingBow;

	//Control input
	[HideInInspector]
	public bool isDodging, isAttacking, isSpecialAttacking, isJumping;
	[HideInInspector]
	public Vector2 moveUniversal;
	[HideInInspector]
	public Vector3 direction;

	//Jump drop attack
	[HideInInspector]
	public GameObject jumpAttackCollider;
	private SlashController jumpAttackController;

	//Arrow attack
	[Header("Arrow")]
	public GameObject arrowConeIndicator;
	[HideInInspector]
	public float bowTimer;

	//Attack
	[HideInInspector]
	public Queue<bool> attackBuffer = new Queue<bool>();
	[HideInInspector]
	public float lastComboTimer;
	[Header("Attack")]
	public Weapon weaponController;
	public SlashController slashCollider;

	//Components
	[HideInInspector]
	public Rigidbody rb;
	[HideInInspector]
	public GroundCheck groundCheck;
	[HideInInspector]
	public Animator anim;
	[HideInInspector]
	public PlayerHealthController healthController;
	[HideInInspector]
	public PowerController powerController;
	[HideInInspector]
	public CustomGravityController gravityController;

	public DetectEnemiesNear detectEnemiesNear;

	public GameObject Trail;

	protected override void Awake()
	{
		base.Awake();
		Trail.SetActive(false);
		if (rb == null) rb = GetComponent<Rigidbody>();
		if (groundCheck == null) groundCheck = GetComponent<GroundCheck>();
		if (anim == null) anim = GetComponent<Animator>();
		if (gravityController == null) gravityController = GetComponent<CustomGravityController>();
		if (healthController == null) healthController = GetComponent<PlayerHealthController>();
		if (powerController == null) powerController = GetComponent<PowerController>();
	}

	protected override void Update()
	{
		base.Update();
		if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
		if (bowTimer >= 0) bowTimer -= Time.deltaTime;
		if (lastComboTimer >= 0) lastComboTimer -= Time.deltaTime;

		direction = new Vector3(this.moveUniversal.x, 0f, this.moveUniversal.y).normalized;
	}

	//Input mando
	public void SetInputVector(Vector2 direction)
	{
		moveUniversal = direction;
	}
	public void SetDodge(bool pressDodge)
	{
		isDodging = pressDodge;
	}
	public void SetAttack(bool pressAttack)
	{
		attackBuffer.Enqueue(pressAttack);
		Trail.SetActive(true);
		Invoke(nameof(RemovAttackBuffer), 0.3f);
	}
	public void SetSpecialAttack(bool pressSpecialAttack)
	{
		isSpecialAttacking = pressSpecialAttack;
	}
	public void SetJump(bool pressJump)
	{
		isJumping = pressJump;
	}

	void RemovAttackBuffer()
	{
		attackBuffer.Dequeue();
	}

	public void EndCombo()
	{
		Trail.SetActive(false);
		ChangeState(typeof(PlayerIdleState));
	}
}
