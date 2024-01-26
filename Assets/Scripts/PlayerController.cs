using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI.Table;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
	//public CharacterController controller;
	[Header("Speed")]
	//Speed
	[SerializeField]
	private float speed;
	[SerializeField]
	private float dodgeSpeed;

	[SerializeField]
	private float turnSmooth;
	float turnSmoothTime;

	[SerializeField]
	private float jumpForce;

	[Header("Stamina")]
	//Stamina
	[SerializeField]
	private float maxStamina;
	[SerializeField]
	private float staminaRegen;
	[SerializeField]
	private float dodgeStamina;
	[SerializeField]
	private float attackStamina;
	[SerializeField]
	private float minBowStamina;
	[SerializeField]
	private float maxBowStamina;
	private float currentBowStamina = 0;

	[SerializeField]
	private float greatSwordAttackStamina;

	private float stamina;

	[Header("Components")]
	//Components
	private Rigidbody rb;
	private Animator anim;
	[SerializeField]
	private StaminaBarController staminaBarC;

	[Header("Timers")]
	//Timers
	[SerializeField]
	private float dodgeCD;
	//[SerializeField]
	//private float invencibilityCD;
	private float dodgeTimer = 0;
	[SerializeField]
	private float dodgeInvencibilitySeconds;

	public float invencibilityTimer = 0;
	private float greatSwordTimePressed = 0;
	[SerializeField]
	private float maxBowCD;
	private float bowCD;

	[Header("Weapons")]
	//Weapon
	[SerializeField]
	private GameObject weapon;
	private Weapon weaponController;

	private int comboCounter;
	float attackCoolDownTime = 0.1f;
	float lastClicked;
	float lastComboEnd;

	[SerializeField]
	private GameObject arrowPrefab;
	[SerializeField]
	private GameObject arrowLineIndicator;
	//[SerializeField]
	//private GameObject indicativeArrow;

	[SerializeField]
	private GameObject hand;

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
	private Vector3 greatSwordAttackDirection = Vector3.zero;

	//Positions&Rotations
	[SerializeField]
	private Transform slashDirection;

	[SerializeField]
	private GameObject slashParticle;

	[SerializeField]
	private GameObject slashCollider;

	private ParticleSystem slashParticleSystem;

	private Vector3 savedPosition;
	private Vector3 savedRotation;

	private PowerController powerController;
	private SlashController slashController;

	/*private bool resetLineArrow = true;
	private bool resetLineArrowAux = true;
	private float resetTimer=0f;*/
	private bool chargingBow = false;

	[SerializeField]
	private GameObject arrowConeIndicator;

	private GameObject arrowline1, arrowline2, arrowline3;

	private bool ground = true;
	private bool jump = false;

	private List<GameObject> enemiesNear = new List<GameObject>();

	private CustomGravityController gravityController;

	private bool nextAttack=false;

	private Queue<bool> attackBuffer = new Queue<bool>();
	void Start()
	{
		slashController = slashCollider.GetComponent<SlashController>();
		powerController = this.GetComponent<PowerController>();
		if (weapon != null) weaponController = weapon.GetComponent<Weapon>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		stamina = maxStamina;
		slashParticleSystem = slashParticle.GetComponent<ParticleSystem>();
		gravityController = this.GetComponent<CustomGravityController>();
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
		//Debug.Log("Entras");
		//isAttacking = pressAttack;
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
		//Movimiento
		direction = new Vector3(moveUniversal.x, 0f, moveUniversal.y).normalized;

		if (direction.magnitude >= 0.1f && !dodge && !attacking)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			isWalking = true;
			//if (attacking) EndCombo();
		}
		else
		{
			isWalking = false;
		}

		Jump();

		//Voltereta
		Roll();
		if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
		if (invencibilityTimer >= 0)
		{
			invencibility = true;
			invencibilityTimer -= Time.deltaTime;
		}
		else invencibility = false;

		//Espada
		//if (!isAttacking && isAttackingAux) isAttackingAux = false; //isAttackingAux se utiliza para detectar que leventas el boton del ataque para volver S atacar
		Attack();
		ExitAttack();

		//Arco
		SpecialAttack();
		if (bowCD >= 0) bowCD -= Time.deltaTime;

		//Stamina
		RecoverStamina();

		//Animaciones
		if (isWalking && !dodge)
		{
			anim.SetBool("Walking", true);
		}
		else if (!isWalking)
		{
			anim.SetBool("Walking", false);
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
		if (isDodging && dodgeTimer <= 0 && !dodge && stamina >= dodgeStamina)
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
			WasteStamina(dodgeStamina);
			anim.SetTrigger("Roll");
			if (attacking) EndCombo();
			invencibilityTimer = dodgeInvencibilitySeconds;
			Invoke("RollEnded", 0.5f); //Por si acaso no entra por animacion

			isDodging = false;
		}
	}

	void RecoverStamina()
	{
		if (stamina < maxStamina && staminaBarC.canRecover)
		{
			stamina += Time.deltaTime * staminaRegen;
			staminaBarC.RecoverBar(stamina / maxStamina);
		}
	}

	void WasteStamina(float wastedStamina)
	{
		stamina -= wastedStamina;
		staminaBarC.SetProgress(stamina / maxStamina, 2);
	}

	void StopAttack()
	{
		attacking = false;
	}

	void WasteStaminaPerSecond()
	{
		stamina -= Time.deltaTime * greatSwordAttackStamina;
		staminaBarC.WasteBar(stamina / maxStamina);
	}

	public void ResetStamina()
	{
		stamina = maxStamina;
		staminaBarC.RecoverBar(stamina / maxStamina);
	}

	void ChangeWeapon(GameObject newWeapon)
	{
		Destroy(weapon);
		weapon = newWeapon;
		weaponController = weapon.GetComponent<Weapon>();
		weapon.GetComponent<BoxCollider>().isTrigger = false;
		weapon.GetComponent<BoxCollider>().enabled = false;
		GameObject parent = weapon.transform.parent.gameObject;
		weapon.transform.parent = hand.transform;
		Destroy(parent);
		weapon.layer = this.gameObject.layer;
		Destroy(weapon.GetComponent<Animator>());
		switch (weapon.tag)
		{

			case "Sword":
				weapon.transform.localPosition = Vector3.zero;
				weapon.transform.localRotation = new Quaternion(0.110803805f, 0.805757701f, 0.131379381f, 0.566759765f);
				break;

			case "GreatSword":
				weapon.transform.localPosition = new Vector3(0.0134290522f, -0.00872362964f, 0.00462706713f);
				weapon.transform.localRotation = new Quaternion(-0.147978142f, 0.552269399f, -0.596118569f, 0.563687623f);
				break;

			case "Bow":
				weapon.transform.localPosition = new Vector3(0.000869999989f, -0.000429999985f, -0.00173999998f);
				weapon.transform.localRotation = new Quaternion(-0.389829606f, -0.491401315f, -0.705046117f, 0.330858946f);
				break;

			default:
				Console.WriteLine("Nothing");
				break;
		}
	}

	public void RollEnded()
	{
		dodge = false;
		//invencibility = false;
	}

	private void ResetVelocity()
	{
		rb.velocity = new Vector3(0,rb.velocity.y,0);
	}

	private Transform TryGetNearestEnemy()
	{
		Transform nearestEnemy = null;

		if (enemiesNear.Count >= 1)
		{
			nearestEnemy = enemiesNear[0].transform;
		}

		return nearestEnemy;
	}

	private void Attack()
	{
		//Forma nueva
		if (attackBuffer.Count>=1 && !dodge)
		{
			//GreatSword and sword
			if (weapon != null)
			{
				if (weapon.tag == "Sword")
				{
					if (Time.time - lastComboEnd > 0.6f && comboCounter < weaponController.combo.Count && stamina >= attackStamina) //Tiempo entre combos
					{
						if (Time.time - lastClicked >= 0.7f) //Tiempo entre ataques
						{
							CancelInvoke("EndCombo");
							
							ResetVelocity();
							//Cosas de slash
							Vector3 savedPosition = slashDirection.position;
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
							mainModule.startRotationY = new ParticleSystem.MinMaxCurve(angleInt);
							//Debug.Log(mainModule.startRotationY.constant);

							mainModule.startRotationY = new ParticleSystem.MinMaxCurve(angleInt);

							WasteStamina(attackStamina);
							attacking = true;
							anim.runtimeAnimatorController = weaponController.combo[comboCounter].animatorOR;
							anim.Play("Attack", 0, 0);
							swordAttackSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
							swordAttackSound.Play();
							slashController.finalDamage = weaponController.combo[comboCounter].damage + powerController.GetCurrentPowerLevel() / 6; //Cambiar escalado poder
																																					//Debug.Log(slashController.finalDamage);
																																					//weaponController.pushForce = weaponController.combo[comboCounter].pushForce;
							slashController.pushForce = weaponController.combo[comboCounter].pushForce;

							Transform target = TryGetNearestEnemy();

							if (target!=null)
							{
								float targetAngle;
								//targetAngle = Mathf.Atan2(target.position.x, target.position.z) * Mathf.Rad2Deg;
								//float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
								//transform.rotation = Quaternion.Euler(0f, angle, 0f);
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
								/*if (target==null)*/ targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
								//else targetAngle = Mathf.Atan2(target.position.x, target.position.z) * Mathf.Rad2Deg;
								float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
								transform.rotation = Quaternion.Euler(0f, angle, 0f);
								if (ground) attackMovement = weaponController.combo[comboCounter].attackMovement;
								else attackMovement = weaponController.combo[comboCounter].attackMovement * 0.5f; //0.5= friccion aire
								moveAttack = true;
							}

							comboCounter++;
							//if (comboCounter == 1) moveAttack = true; //solo hace el dash la primera vez

							gravityController.gravityOn = false;
							lastClicked = Time.time;
							/*if (comboCounter >= weaponController.combo.Count)
							{
								comboCounter = 0;
								lastComboEnd = Time.time;
							}*/

							slashCollider.SetActive(false);
							slashParticle.SetActive(false);

							StartCoroutine(ReactivateObjects());
							//isAttacking = false;

							this.gameObject.transform.DOPunchScale(new Vector3(0.3f, -0.3f, 0.3f), 0.3f).SetRelative(true).SetEase(Ease.OutBack);
						}
					}
				}
			}
		}

	}

	IEnumerator ReactivateObjects()
	{
		yield return new WaitForSeconds(0.05f); // Ajusta el tiempo según sea necesario

		//slashCollider.SetActive(true);
		//slashParticle.SetActive(true);

		yield return new WaitForSeconds(0.4f); // Ajusta el tiempo según sea necesario
		slashCollider.SetActive(false);
		slashParticle.SetActive(false);
	}

	private void SpecialAttack()
	{
		if (isSpecialAttacking && !dodge)
		{
			if (bowCD <= 0) //CD de ataque con arco
			{
				if (stamina > 0 && currentBowStamina < maxBowStamina) //Mira si tienes stamina para seguir cargando el arco y si puedes seguir cargandolo mas
				{
					chargingBow = true;
					anim.SetBool("Bow", true);

					if (!onlySoundOnce)
					{
						tensingBow.Play();
						tensingBow.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
						onlySoundOnce = true;
					}

					WasteStaminaPerSecond();
					currentBowStamina += Time.deltaTime;

					//remove gravity
					gravityController.gravityOn = false;
					/*if (resetTimer > 0 && resetLineArrowAux) resetTimer -= Time.deltaTime;
					
                    if (resetLineArrowAux && resetTimer<=0)
					{
						resetLineArrowAux = false;
						resetLineArrow = true;
					}*/

					if (direction != Vector3.zero) //Hacer que puedas rotar mientras cargas
					{
						float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
						float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
						transform.rotation = Quaternion.Euler(0f, angle, 0f);
						//resetLineArrow = true;
						/*if (resetTimer <= 0)
                        {
							resetTimer = 0.2f;
                            resetLineArrowAux = true;
                        }*/
					}

					attacking = true;
					//indicativeArrow.SetActive(true);
					arrowConeIndicator.SetActive(true);

					//Linea cuando carga
					/*if (resetLineArrow)
                    {
						Destroy(arrowline1);
						Destroy(arrowline2);
						Destroy(arrowline3);

						Quaternion rot = this.transform.rotation;
                        arrowline1 = Instantiate(arrowLineIndicator, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.transform.rotation);
                        //arrowline1.transform.parent = this.gameObject.transform;
                        Vector3 cone1 = rot.eulerAngles + new Vector3(0, 5, 0);
                        Vector3 cone2 = rot.eulerAngles + new Vector3(0, -5, 0);

                        arrowline2 = Instantiate(arrowLineIndicator, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rot);
                        arrowline2.transform.eulerAngles = cone1;
						//arrowline2.transform.parent = this.gameObject.transform;
						arrowline3 = Instantiate(arrowLineIndicator, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), rot);
                        arrowline3.transform.eulerAngles = cone2;
						//arrowline3.transform.parent = this.gameObject.transform;
						resetLineArrow = false;
                    }*/
				}
				else if (currentBowStamina >= minBowStamina) //Si ya no le queda estamina o ha tensado el arco almenos hasta lo minimo
				{
					ShootArrow();
					onlySoundOnce = false;
				}
				else //Si no ha tensado el arco hasta lo minimo no lanza las flechas
				{
					anim.SetBool("Bow", false);
					currentBowStamina = 0;
					attacking = false;
					chargingBow = false;
					//indicativeArrow.SetActive(false);
					arrowConeIndicator.SetActive(false);
					Destroy(arrowline1);
					Destroy(arrowline2);
					Destroy(arrowline3);
					onlySoundOnce = false;
					gravityController.gravityOn = true;
				}
			}
		}
		else if (!isSpecialAttacking && currentBowStamina >= minBowStamina) //Ha dejado de apretar el boton, pero ya lo habia comenzado a cargar almenos hasta lo minimo
		{
			ShootArrow();
			onlySoundOnce = false;
		}
		else if (!isSpecialAttacking && currentBowStamina > 0 && currentBowStamina < minBowStamina) //Ha dejado de apretar el boton, pero ya lo habia comenzado a cargar sin llegar al minimo, no lanza flechas
		{
			anim.SetBool("Bow", false);
			currentBowStamina = 0;
			//Debug.Log("asdadasdasd");
			attacking = false;
			chargingBow = false;
			//indicativeArrow.SetActive(false);
			arrowConeIndicator.SetActive(false);
			Destroy(arrowline1);
			Destroy(arrowline2);
			Destroy(arrowline3);
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
		ac.SetSpeed(currentBowStamina * 60);
		ac.SetPushForce(currentBowStamina * 70);
		ac.owner = this.gameObject;

		ArrowController ac2 = arrow2.GetComponent<ArrowController>();

		ac2.finalDamage = ac2.baseDamage + powerController.GetCurrentPowerLevel() / 6; //cambiar escalado de poder
		ac2.SetSpeed(currentBowStamina * 60);
		ac2.SetPushForce(currentBowStamina * 70);
		ac2.owner = this.gameObject;

		ArrowController ac3 = arrow3.GetComponent<ArrowController>();

		ac3.finalDamage = ac3.baseDamage + powerController.GetCurrentPowerLevel() / 6; //cambiar escalado de poder
		ac3.SetSpeed(currentBowStamina * 60);
		ac3.SetPushForce(currentBowStamina * 70);
		ac3.owner = this.gameObject;
		Invoke("StopAttack", 0.3f);
		chargingBow = false;
		//indicativeArrow.SetActive(false);
		arrowConeIndicator.SetActive(false);
		Destroy(arrowline1);
		Destroy(arrowline2);
		Destroy(arrowline3);
		currentBowStamina = 0;
		bowCD = maxBowCD;
		anim.SetBool("Bow", false);
		bowAttackSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		bowAttackSound.Play();
		//resetLineArrow = true;
		gravityController.gravityOn = true;
	}

	private void ExitAttack()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			//slashCollider.SetActive(false);
			//slashParticle.SetActive(false);
			//rb.velocity = Vector3.zero;
			//Debug.Log("Entras aquii??2");
			anim.SetTrigger("Attack");
			ResetVelocity();
			gravityController.gravityOn = true;
			Invoke("EndCombo", 0.5f);
		}
	}

	private void EndCombo()
	{
		anim.ResetTrigger("Attack");
		slashCollider.SetActive(false);
		slashParticle.SetActive(false);
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
		// Verifica si el objeto que entró tiene la etiqueta "Enemigo".
		if (other.CompareTag("Enemy"))
		{
			Debug.Log("Entra enemigo");
			enemiesNear.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		// Verifica si el objeto que entró tiene la etiqueta "Enemigo".
		if (other.CompareTag("Enemy"))
		{
			Debug.Log("Sale enemigo");
			enemiesNear.Remove(other.gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			ground = true;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			ground = false;
		}
	}
}
