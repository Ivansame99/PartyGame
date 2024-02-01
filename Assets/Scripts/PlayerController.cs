using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : PlayerStateManager<PlayerController>
{
	[SerializeField]
	private float dodgeSpeed;

	[SerializeField]
	private float jumpForce;

	[Header("Components")]
	//Components
	private Rigidbody rb;
	private Animator anim;

	[Header("Timers")]
	[SerializeField]
	private float dodgeCD;
	private float dodgeTimer = 0;
	[SerializeField]
	private float dodgeInvencibilitySeconds;
	public float invencibilityTimer = 0;
	[SerializeField]
	private float maxBowCD;
	private float bowCD;

	[Header("Weapons")]
	[SerializeField]
	private GameObject weapon;
	private Weapon weaponController;

	private float currentChargingBow;
	[SerializeField] private float minChargeBow;
	[SerializeField] private float maxChargeBow;

	private int comboCounter;
	float attackCoolDownTime = 0.1f;
	float lastClicked;
	float lastComboEnd;

	[SerializeField]
	private GameObject arrowPrefab;

	private float attackMovement;

	[Header("Audio")]
	//AUDIO
	[SerializeField]
	private AudioSource swordAttackSound;
	[SerializeField]
	private AudioSource bowAttackSound;
	[SerializeField] AudioSource dodgeSound;
	[SerializeField] AudioSource tensingBow;
	[SerializeField] private float minPitch;
	[SerializeField] private float maxPitch;
	private bool onlySoundOnce = false;

	//States
	public bool invencibility = false;
	public bool dodge = false;
	private bool isWalking = false;
	private bool attacking = false;
	private bool moveAttack = false;
	private int greatSwordAttackState = 0;
	private bool isCharging = false;

	//Control
	private bool isDodging, isAttacking, isSpecialAttacking, isJumping;

	//Movement
	[HideInInspector]
	public Vector2 moveUniversal;
	private Vector3 direction;
	private Vector3 rollDirection;

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

	private PowerController powerController;
	//private SlashController slashController;

	private bool chargingBow = false;

	[SerializeField]
	private GameObject arrowConeIndicator;

	private bool jump = false;

	private List<GameObject> enemiesNear = new List<GameObject>();

	private CustomGravityController gravityController;

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

	private GroundCheck groundCheck;

	protected override void Awake()
	{
		base.Awake();
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
