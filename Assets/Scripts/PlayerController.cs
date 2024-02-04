using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : PlayerStateManager<PlayerController>
{
	[Header("Timers")]
	[HideInInspector]
	public float dodgeTimer = 0;
	public float invencibilityTimer = 0;

	[HideInInspector]
	public float bowTimer;

	public Weapon weaponController;

	[Header("Audio")]
	public AudioSource swordAttackSound;
	public AudioSource bowAttackSound;
	public AudioSource dodgeSound;
	public AudioSource tensingBow;

	//States
	public bool invencibility = false;
	public bool dodge = false;

	//Control
	[HideInInspector]
	public bool isDodging, isAttacking, isSpecialAttacking, isJumping;

	//Movement
	[HideInInspector]
	public Vector2 moveUniversal;
	[HideInInspector]
	public Vector3 direction;

	public SlashController slashCollider;
	
	[HideInInspector]
	public float lastComboTimer;

	[HideInInspector]
	public GameObject jumpAttackCollider;
	private SlashController jumpAttackController;

	[HideInInspector]
	public PowerController powerController;

	public GameObject arrowConeIndicator;

	[HideInInspector]
	public List<GameObject> enemiesNear = new List<GameObject>();

	[HideInInspector]
	public CustomGravityController gravityController;
	
	[HideInInspector]
	public Queue<bool> attackBuffer = new Queue<bool>();

	[HideInInspector]
	public Rigidbody rb;
	[HideInInspector]
	public GroundCheck groundCheck;
	[HideInInspector]
	public Animator anim;

	protected override void Awake()
	{
		base.Awake();
		if (rb == null) rb = GetComponent<Rigidbody>();
		if (groundCheck == null) groundCheck = GetComponent<GroundCheck>();
		if (anim == null) anim = GetComponent<Animator>();
		if (gravityController ==null) gravityController = GetComponent<CustomGravityController>();
		if (powerController == null) powerController = GetComponent<PowerController>();
	}

	protected override void Update()
	{
		base.Update();
		if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
		if (bowTimer >= 0) bowTimer -= Time.deltaTime;
		if(lastComboTimer>=0) lastComboTimer -= Time.deltaTime;

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
		ChangeState(typeof(PlayerIdleState));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy") || other.CompareTag("Player"))
		{
			enemiesNear.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Enemy") || other.CompareTag("Player"))
		{
			enemiesNear.Remove(other.gameObject);
		}
	}
}
