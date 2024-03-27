using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : PlayerStateManager<PlayerController>
{
	#region Inspector Variables
	[SerializeField]
	private int playerId;

	public GameObject jumpAttackCollider;

	public GameObject arrowConeIndicator;

	public Weapon weaponController;

	public SlashController slashCollider;

	public DetectEnemiesNear detectEnemiesNear;

	[SerializeField]
	private GameObject sword;

	[SerializeField]
	private GameObject bow;

	[SerializeField]
	private GameObject ak47;
	#endregion

	#region Variables
	//Components
	internal Rigidbody rb;
	internal GroundCheck groundCheck;
	internal Animator anim;
	internal PlayerHealthController healthController;
	internal PowerController powerController;
	internal CustomGravityController gravityController;
	internal WaterDetection waterDetection;

	//Timers
	internal float dodgeTimer = 0;
	internal float bowTimer;
	internal float lastComboTimer;

	//Control input
	internal bool isDodging, isAttacking, isSpecialAttacking, isJumping;
	internal Vector2 moveUniversal;
	internal Vector3 direction;
	internal PlayerAudioManager playerAudioManager;

	//Logic
	internal Queue<bool> attackBuffer = new Queue<bool>();
	public bool ak;
	#endregion


	#region Life Cycle
	protected override void Awake()
	{
		base.Awake();
	
		if (rb == null) rb = GetComponent<Rigidbody>();
		if (groundCheck == null) groundCheck = GetComponent<GroundCheck>();
		if (anim == null) anim = GetComponent<Animator>();
		if (gravityController == null) gravityController = GetComponent<CustomGravityController>();
		if (healthController == null) healthController = GetComponent<PlayerHealthController>();
		if (powerController == null) powerController = GetComponent<PowerController>();
		if (waterDetection == null) waterDetection = GetComponent<WaterDetection>();
		if(playerAudioManager == null) playerAudioManager = GetComponent<PlayerAudioManager>();
	}

	protected override void Update()
	{
		base.Update();
		if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
		if (bowTimer >= 0) bowTimer -= Time.deltaTime;
		if (lastComboTimer >= 0) lastComboTimer -= Time.deltaTime;

		direction = new Vector3(this.moveUniversal.x, 0f, this.moveUniversal.y).normalized;
	}
	#endregion

	#region Private Methods
	void RemovAttackBuffer()
	{
		attackBuffer.Dequeue();
	}
	#endregion

	#region Public Methods
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

	public void SetPause(bool pause)
	{
		return;
	}

	public void EndCombo()
	{
		ChangeState(typeof(PlayerIdleState));
	}

	public int GetPlayerId()
	{
		return playerId;
	}

	public void EquipAk()
	{
		sword.SetActive(false);
		bow.SetActive(false);
		ak47.SetActive(true);
	}

	#endregion
}
