using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
	[Header("Speed")]
	[SerializeField]
	private float speed;
	[SerializeField]
	private float dodgeSpeed;

	[SerializeField]
	private float turnSmooth;
	float turnSmoothTime;

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
	Vector2 moveUniversal;
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

	//private ParticleSystem slashParticleSystem;

	//private Vector3 savedPosition;
	//private Vector3 savedRotation;

	private PowerController powerController;
	//private SlashController slashController;

	private bool chargingBow = false;

	[SerializeField]
	private GameObject arrowConeIndicator;

	private float raycastDistance = 1.2f; // Distancia del Raycast
	public LayerMask groundLayer; // Capas que representan el suelo
	private bool ground = true;
	private bool jump = false;

	private List<GameObject> enemiesNear = new List<GameObject>();

	private CustomGravityController gravityController;

	private bool nextAttack = false;

	private Queue<bool> attackBuffer = new Queue<bool>();

	private bool canAttackNext = true;

	private bool exitAttack = false;

	private float originalGravityScale;

	private bool staticPJ = false;
	void Start()
	{
		//slashController = slashCollider.GetComponent<SlashController>();
		powerController = this.GetComponent<PowerController>();
		if (weapon != null) weaponController = weapon.GetComponent<Weapon>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		//slashParticleSystem = slashParticle.GetComponent<ParticleSystem>();
		gravityController = this.GetComponent<CustomGravityController>();
		originalGravityScale = gravityController.gravityScale;
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

	void Update()
	{
		DetectGround();

		//Invencibilidad
		if (invencibilityTimer >= 0)
		{
			invencibility = true;
			invencibilityTimer -= Time.deltaTime;
		}
		else invencibility = false;

		//Animaciones
		if (isWalking && !dodge)
		{
			anim.SetBool("Walking", true);
		}
		else if (!isWalking)
		{
			anim.SetBool("Walking", false);
		}

		if (!staticPJ)
		{
			//Movimiento
			direction = new Vector3(moveUniversal.x, 0f, moveUniversal.y).normalized;

			if (direction.magnitude >= 0.1f && !dodge && !attacking)
			{
				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);
				isWalking = true;
			}
			else
			{
				isWalking = false;
			}

			Jump();

			JumpAttack();

			//Voltereta
			Roll();

			Attack();
			ExitAttack();

			//Arco
			SpecialAttack();
			if (bowCD >= 0) bowCD -= Time.deltaTime;


		}
	}

	void DetectGround()
	{
		Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);

		if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
		{
			ground = true;
		}
		else
		{
			ground = false;
		}
	}

	void Jump()
	{
		if (ground && isJumping && !jump)
		{
			this.gameObject.transform.DOPunchScale(new Vector3(1f, -1f, 1f), 0.7f).SetRelative(true).SetEase(Ease.OutBack);
			jump = true;
			isJumping = false;
		}
	}

	void RemovAttackBuffer()
	{
		attackBuffer.Dequeue();
	}

	void Roll()
	{
		if (isDodging && dodgeTimer <= 0 && !dodge && ground)
		{
			if (direction == Vector3.zero)
			{
				rollDirection = this.transform.forward;
			}
			else
			{
				rollDirection = direction;
				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);
			}
			swordAttackSound.pitch = UnityEngine.Random.Range(1.5f, 1.9f);
			dodgeSound.Play();
			dodge = true;
			dodgeTimer = dodgeCD;
			anim.SetTrigger("Roll");
			if (attacking) EndCombo();
			invencibilityTimer = dodgeInvencibilitySeconds;
			Invoke("RollEnded", 0.5f); //Por si acaso no entra por animacion

			isDodging = false;
		}

		if (dodge) anim.SetTrigger("Roll");
		if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
	}

	void StopAttack()
	{
		attacking = false;
	}

	public void RollEnded()
	{
		anim.ResetTrigger("Roll");
		dodge = false;
	}

	private void ResetVelocity()
	{
		rb.velocity = new Vector3(0, rb.velocity.y, 0);
	}

	private Transform TryGetNearestEnemy()
	{
		Transform nearestEnemy = null;
		float nearestEnemyDistance = float.MaxValue;

		if (enemiesNear.Count >= 1)
		{
			foreach (GameObject enemy in enemiesNear)
			{
				if (enemy == null)
				{
					enemiesNear.Remove(enemy);
					if (enemiesNear.Count == 0) return null;
					continue;
				}

				Vector3 enemyDistanceDiff = enemy.transform.position - this.transform.position;
				float enemyDistance = enemyDistanceDiff.sqrMagnitude;

				if (enemyDistance < nearestEnemyDistance)
				{
					nearestEnemyDistance = enemyDistance;
					nearestEnemy = enemy.transform;
				}
			}
		}

		return nearestEnemy;
	}

	private void Attack()
	{
		if (attackBuffer.Count >= 1 && !dodge)
		{
			if (weapon != null)
			{
				if (Time.time - lastComboEnd > 0.4f && comboCounter < weaponController.combo.Count) //Tiempo entre combos
				{
					if (Time.time - lastClicked >= 0.4f) //Tiempo entre ataques
					{
						CancelInvoke("EndCombo");

						ResetVelocity();
						//Cosas de slash
						/*Vector3 savedPosition = slashDirection.position;
						slashParticle.transform.position = savedPosition;
						slashCollider.transform.position = savedPosition;

						Vector3 forwardDirection = slashDirection.forward;
						Quaternion lookRotation = Quaternion.LookRotation(forwardDirection, slashDirection.up);

						var mainModule = slashParticleSystem.main;
						mainModule.startRotationY = 0;
						float newAngle = lookRotation.eulerAngles.y;
						newAngle = Mathf.Repeat(newAngle, 360f);
						int angleInt = Mathf.FloorToInt(newAngle);

						// LO DE ABAJO FUNCIONA PERO HAY QUE HACERLO BIEN PORQUE ES UNA PUTA MIERDA

						if (angleInt < 0) angleInt = angleInt * -1;
						if (angleInt >= 360) angleInt = 360;
						if (angleInt > 5 && angleInt <= 29) angleInt = 20;
						if (angleInt >= 30 && angleInt <= 50) angleInt = 40;
						if (angleInt >= 0 && angleInt <= 5) angleInt = 360;
						if (angleInt >= 51 && angleInt <= 69) angleInt = 60;
						if (angleInt >= 70 && angleInt <= 139) angleInt = 135;
						if (angleInt >= 140 && angleInt <= 169) angleInt = 160; // ESTA LA HACE RARA
						if (angleInt >= 170 && angleInt <= 200) angleInt = 200;
						if (angleInt >= 201 && angleInt <= 260) angleInt = 250;
						if (angleInt >= 261 && angleInt <= 280) angleInt = 270;
						if (angleInt >= 281 && angleInt <= 350) angleInt = 340;
						if (angleInt >= 351 && angleInt <= 359) angleInt = 360;
						mainModule.startRotationY = new ParticleSystem.MinMaxCurve(angleInt);*/
						//Debug.Log(mainModule.startRotationY.constant);

						//mainModule.startRotationY = new ParticleSystem.MinMaxCurve(angleInt);

						attacking = true;
						anim.runtimeAnimatorController = weaponController.combo[comboCounter].animatorOR;
						anim.Play("Attack", 0, 0);
						swordAttackSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
						swordAttackSound.Play();
						slashCollider.finalDamage = weaponController.combo[comboCounter].damage + powerController.GetCurrentPowerLevel() / 6; //Cambiar escalado poder
																																			  //Debug.Log(slashController.finalDamage);
																																			  //weaponController.pushForce = weaponController.combo[comboCounter].pushForce;
						slashCollider.pushForce = weaponController.combo[comboCounter].pushForce;

						Transform target = TryGetNearestEnemy();

						if (target != null)
						{
							float targetAngle;
							Vector3 direction = target.position - transform.position;
							direction.y = 0; // Establece la dirección en el eje Y a 0 para mantener al personaje vertical
							Quaternion rotation = Quaternion.LookRotation(direction);
							transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
							if (ground) attackMovement = weaponController.combo[comboCounter].attackMovement;
							else attackMovement = weaponController.combo[comboCounter].attackMovement * 0.5f; //0.5= friccion aire
							moveAttack = true;
						}


						//Solo se hace el dash al atacar si se esta moviendo, si no ataca en el sitio
						if (direction.magnitude >= 0.1f)
						{
							float targetAngle;
							targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
							float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
							transform.rotation = Quaternion.Euler(0f, angle, 0f);
							if (ground) attackMovement = weaponController.combo[comboCounter].attackMovement;
							else attackMovement = weaponController.combo[comboCounter].attackMovement * 0.5f; //0.5= friccion aire
							moveAttack = true;
						}

						comboCounter++;

						gravityController.gravityOn = false;
						lastClicked = Time.time;

						//slashCollider.SetActive(false);
						//slashParticle.SetActive(false);

						//StartCoroutine(ReactivateObjects());

						this.gameObject.transform.DOPunchScale(new Vector3(0.6f, -0.6f, 0.6f), 0.6f).SetRelative(true).SetEase(Ease.OutBack);

						exitAttack = true;
					}
				}
			}
		}

	}

	/*IEnumerator ReactivateObjects()
	{
		yield return new WaitForSeconds(0.05f); // Ajusta el tiempo según sea necesario

		yield return new WaitForSeconds(0.4f); // Ajusta el tiempo según sea necesario
		slashCollider.SetActive(false);
		slashParticle.SetActive(false);
	}*/

	private void JumpAttack()
	{
		if (!ground && isDodging)
		{
			//dodgeTimer = 3f;
			StartCoroutine(IJumpAttack());
		}
	}

	private IEnumerator IJumpAttack()
	{
		gravityController.gravityOn = false;
		yield return new WaitForSeconds(0.1f);
		gravityController.gravityOn = true;
		gravityController.gravityScale = 20f;
		staticPJ = true;
		isWalking = false;
		//bool groundAux = false;

		while (!ground)
		{
			yield return null;
		}

		//yield return new WaitForSeconds(0.7f);
		gravityController.gravityScale = originalGravityScale;
		jumpAttackCollider.SetActive(true);

		yield return new WaitForSeconds(0.2f);
		jumpAttackCollider.SetActive(false);

		yield return new WaitForSeconds(1f);

		staticPJ = false;
	}

	private void SpecialAttack()
	{
		if (isSpecialAttacking && !dodge)
		{
			if (bowCD <= 0) //CD de ataque con arco
			{
				if (currentChargingBow < maxChargeBow) //Mira si tienes stamina para seguir cargando el arco y si puedes seguir cargandolo mas
				{
					chargingBow = true;
					anim.SetBool("Bow", true);

					if (!onlySoundOnce)
					{
						tensingBow.Play();
						tensingBow.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
						onlySoundOnce = true;
					}

					currentChargingBow += Time.deltaTime;

					//remove gravity
					gravityController.gravityOn = false;

					if (direction != Vector3.zero) //Hacer que puedas rotar mientras cargas
					{
						float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
						float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
						transform.rotation = Quaternion.Euler(0f, angle, 0f);
					}

					attacking = true;
					arrowConeIndicator.SetActive(true);
				}
				else //Ya ha tensado el arco al maximo
				{
					ShootArrow();
					onlySoundOnce = false;
				}
			}
		}
		else if (!isSpecialAttacking && currentChargingBow >= minChargeBow) //Ha dejado de apretar el boton, pero ya lo habia comenzado a cargar almenos hasta lo minimo
		{
			ShootArrow();
			onlySoundOnce = false;
		}
		else if (!isSpecialAttacking && currentChargingBow > 0 && currentChargingBow < minChargeBow) //Ha dejado de apretar el boton, pero ya lo habia comenzado a cargar sin llegar al minimo, no lanza flechas
		{
			anim.SetBool("Bow", false);
			currentChargingBow = 0;
			attacking = false;
			chargingBow = false;
			arrowConeIndicator.SetActive(false);
			onlySoundOnce = true;
			gravityController.gravityOn = true;
		}
	}

	void ShootArrow()
	{
		Quaternion rot = this.transform.rotation;

		GameObject arrow1 = Instantiate(arrowPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);

		Vector3 cone1 = rot.eulerAngles + new Vector3(0, 5, 0);
		Vector3 cone2 = rot.eulerAngles + new Vector3(0, -5, 0);

		GameObject arrow2 = Instantiate(arrowPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rot);
		arrow2.transform.eulerAngles = cone1;

		GameObject arrow3 = Instantiate(arrowPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rot);
		arrow3.transform.eulerAngles = cone2;

		ArrowController ac = arrow1.GetComponent<ArrowController>();

		ac.finalDamage = ac.baseDamage + powerController.GetCurrentPowerLevel() / 6; //cambiar escalado de poder
		ac.SetSpeed(currentChargingBow * 60);
		ac.SetPushForce(currentChargingBow * 70);
		ac.owner = this.gameObject;

		ArrowController ac2 = arrow2.GetComponent<ArrowController>();

		ac2.finalDamage = ac2.baseDamage + powerController.GetCurrentPowerLevel() / 6; //cambiar escalado de poder
		ac2.SetSpeed(currentChargingBow * 60);
		ac2.SetPushForce(currentChargingBow * 70);
		ac2.owner = this.gameObject;

		ArrowController ac3 = arrow3.GetComponent<ArrowController>();

		ac3.finalDamage = ac3.baseDamage + powerController.GetCurrentPowerLevel() / 6; //cambiar escalado de poder
		ac3.SetSpeed(currentChargingBow * 60);
		ac3.SetPushForce(currentChargingBow * 70);
		ac3.owner = this.gameObject;
		Invoke("StopAttack", 0.3f);
		chargingBow = false;
		//indicativeArrow.SetActive(false);
		arrowConeIndicator.SetActive(false);
		currentChargingBow = 0;
		bowCD = maxBowCD;
		anim.SetBool("Bow", false);
		bowAttackSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		bowAttackSound.Play();
		//resetLineArrow = true;
		gravityController.gravityOn = true;
	}

	private void ExitAttack()
	{
		if (exitAttack && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			anim.SetTrigger("Attack");
			ResetVelocity();
			gravityController.gravityOn = true;
			Invoke("EndCombo", 0.2f);
			exitAttack = false;
		}
	}

	private void EndCombo()
	{
		anim.ResetTrigger("Attack");
		//slashCollider.SetActive(false);
		//slashParticle.SetActive(false);
		ResetVelocity();
		gravityController.gravityOn = true;
		attacking = false;
		comboCounter = 0;
		lastComboEnd = Time.time;
	}

	private void FixedUpdate()
	{
		if (chargingBow && ground)
		{
			rb.MovePosition(transform.position + direction * speed / 2 * Time.fixedDeltaTime);
		}
		else if (dodge)
		{
			rb.MovePosition(transform.position + rollDirection * dodgeSpeed * Time.fixedDeltaTime);
		}
		else if (isWalking)
		{
			rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
		}

		if (jump)
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			jump = false;
		}

		if (moveAttack)
		{
			rb.AddForce(transform.forward * attackMovement, ForceMode.Impulse);
			moveAttack = false;
		}
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
