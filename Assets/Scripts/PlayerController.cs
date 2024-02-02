using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : PlayerStateManager<PlayerController>
{
	//[SerializeField]
	//private float dodgeSpeed;

	//[Header("Components")]
	//Components
	//private Rigidbody rb;
	//private Animator anim;

	[Header("Timers")]
	//[SerializeField]
	//private float dodgeCD;
	[HideInInspector]
	public float dodgeTimer = 0;
	[SerializeField]
	private float dodgeInvencibilitySeconds;
	public float invencibilityTimer = 0;

	[HideInInspector]
	public float bowTimer;

	[Header("Weapons")]
	[SerializeField]
	private GameObject weapon;
	private Weapon weaponController;

	/*private float currentChargingBow;
	[SerializeField] private float minChargeBow;
	[SerializeField] private float maxChargeBow;*/

	private int comboCounter;
	float attackCoolDownTime = 0.1f;
	float lastClicked;
	float lastComboEnd;

	//[SerializeField]
	//private GameObject arrowPrefab;

	private float attackMovement;

	[Header("Audio")]
	public AudioSource swordAttackSound;
	public AudioSource bowAttackSound;
	public AudioSource dodgeSound;
	public AudioSource tensingBow;
	[SerializeField] private float minPitch;
	[SerializeField] private float maxPitch;
	private bool onlySoundOnce = false;

	//States
	public bool invencibility = false;
	public bool dodge = false;
	private bool isWalking = false;
	private bool attacking = false;
	private bool moveAttack = false;
	//private int greatSwordAttackState = 0;
	private bool isCharging = false;

	//Control
	[HideInInspector]
	public bool isDodging, isAttacking, isSpecialAttacking, isJumping;

	//Movement
	[HideInInspector]
	public Vector2 moveUniversal;
	[HideInInspector]
	public Vector3 direction;
	//private Vector3 rollDirection;

	//Positions&Rotations
	//[SerializeField]
	//private Transform slashDirection;

	//[SerializeField]
	//private GameObject slashParticle;

	[SerializeField]
	private SlashController slashCollider;

	[SerializeField]
	private GameObject jumpAttackCollider;
	private SlashController jumpAttackController;

	//private ParticleSystem slashParticleSystem;

	//private Vector3 savedPosition;
	//private Vector3 savedRotation;
	[HideInInspector]
	public PowerController powerController;
	//private SlashController slashController;

	private bool chargingBow = false;

	public GameObject arrowConeIndicator;

	private List<GameObject> enemiesNear = new List<GameObject>();

	[HideInInspector]
	public CustomGravityController gravityController;

	private bool nextAttack = false;

	private Queue<bool> attackBuffer = new Queue<bool>();

	private bool canAttackNext = true;

	private bool exitAttack = false;

	private float originalGravityScale;

	private bool staticPJ = false;

	[SerializeField]
	private GameObject fallParticle;

	private bool fallParticleBool = false;

	[SerializeField]
	private GameObject runParticles;

	private float runCounter = 0;
	private float runCounterRandom;

	private bool littleMove;

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
		//Invoke(nameof(RemovAttackBuffer), 0.3f);
	}
	public void SetSpecialAttack(bool pressSpecialAttack)
	{
		isSpecialAttacking = pressSpecialAttack;
	}
	public void SetJump(bool pressJump)
	{
		isJumping = pressJump;
	}
}
