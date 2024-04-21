using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHealthController : MonoBehaviour
{
	#region Inspector Variables
	[Header("Logic")]
	private float health;

	[SerializeField]
	private float maxHealthBase;

	[SerializeField]
	private float inmuneTime;

	[Header("Feedback")]
	[SerializeField] private GameObject cross1;
	[SerializeField] private GameObject cross2;
	[SerializeField] private GameObject glow;

	[SerializeField] private GameObject deathParticles;
	[SerializeField] private GameObject bloodParticles;
	[SerializeField] private GameObject hitParticles;

	[SerializeField] private HelmetPrefab[] dieDrops;
	[SerializeField] private float minForce = 5f;
	[SerializeField] private float maxForce = 10f;

	[SerializeField] private GameObject ghostPrefab;

	[SerializeField] private Material redMaterial;
	[SerializeField] private Renderer helmet;
	[SerializeField] private Renderer body;
	#endregion

	#region Variables
	private PlayerConfiguration playerConfig;
	private GameObject ghost;
	private float maxHealth;

	private bool deadAux = false;

	private GameObject lastAttacker;
	private float currentPower;

	private PlayersHealthManager playersHealthManager;

	private bool pushBack;
	private Vector3 attackPosition;
	private float pushForce;

	private PlayerController playerController;
	private PowerController powerController;
	private PlayerHudController playerHudController;

	private Animator anim;
	private Rigidbody rb;

	internal bool dead = false;
	internal float invencibleTimer;

	private Material originalHelmetMaterial;
	private Material originalBodyMaterial;
	#endregion

	#region Life Cycle
	private void Awake()
	{
		//Components
		playerController = GetComponent<PlayerController>();
		powerController = GetComponent<PowerController>();
		playerHudController = GetComponent<PlayerHudController>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		playersHealthManager = GameManager.Instance.playersHealthManager;
		playerConfig = this.GetComponent<PlayerInputHandler>().playerConfig;

		originalHelmetMaterial = helmet.material;
		originalBodyMaterial = body.material;

		maxHealth = maxHealthBase + powerController.PowerHealth();
		powerController.OnCurrentPowerChanged += HandleCurrentPowerChanged;
		health = maxHealth;
	}

	void Update()
	{
		if (invencibleTimer >= 0)
		{
			invencibleTimer -= Time.deltaTime;
		}

		if (dead == true && !deadAux)
		{
			deadAux = true;
			//StartCoroutine(ScaleUpAndDown(this.transform, new Vector3(0f, 0f, 0f), 1f));
		}
	}

	private void FixedUpdate()
	{
		if (pushBack)
		{
			Vector3 direction = (this.transform.position - attackPosition).normalized;
			direction.y = 0;
			rb.AddForce(direction * pushForce, ForceMode.Impulse);
			pushBack = false;
		}
	}
	#endregion

	#region Public Methods
	public void ReceiveDamage(float damage)
	{
		DamageFeedback();

		//Sound
		playerController.playerAudioManager.PlayDamage();

		//Logic
		health -= damage;
		if (health <= 0) Die();
		invencibleTimer = inmuneTime;

		playerHudController.ReceivedDamage(damage, health, maxHealth);
	}

	public void ReceiveDamageMultiplier(float multiplier)
	{
		DamageFeedback();

		float damage = maxHealth * multiplier;

		//Sound
		playerController.playerAudioManager.PlayDamage();

		//Logic
		health -= damage;
		if (health <= 0) Die();
		invencibleTimer = inmuneTime;

		playerHudController.ReceivedDamage(damage, health, maxHealth);
	}

	public void RestoreHealthAfterRound()
	{
		float roundHealthAmmount = 0.3f;
		float totalHealth = maxHealth * roundHealthAmmount;
		health += totalHealth;
		if (health >= maxHealth) health = maxHealth;
		playerHudController.ChangeUIHealth(health, maxHealth);
	}

	public void EnablePlayer()
	{
		if (ghost != null)
		{
			ghost.GetComponent<Animator>().SetTrigger("GhostDeath");
			Destroy(ghost, 0.5f);
		}

		powerController.enabled = true;
		playerController.enabled = true;
		playerHudController.EnableHud();
		dead = false;
		deadAux = false;
		health = maxHealth;
		invencibleTimer = 0.5f;
		playerHudController.ChangeUIHealth(health, maxHealth);
		powerController.ChangeScale();
	}
	#endregion

	#region Private Methods
	private void DamageFeedback()
	{
		Instantiate(hitParticles, this.transform.position, Quaternion.identity);
				StartCoroutine(RedEffect());
		Instantiate(bloodParticles, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);

		cross1.SetActive(false);
		cross2.SetActive(false);
		glow.SetActive(false);

		cross1.SetActive(true);
		cross2.SetActive(true);
		glow.SetActive(true);
	}
	
	private void Die()
	{
		//Feedback
		foreach (var helmetPrefab in dieDrops)
		{
			if (Random.value <= helmetPrefab.spawnChance)
			{
				float yOffset = 2f;
				Vector3 playerUpPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
				Vector3 spawnPosition = playerUpPos + Random.insideUnitSphere;
				GameObject helmetInstance = Instantiate(helmetPrefab.prefab, spawnPosition, Quaternion.identity);
				Rigidbody helmetRigidbody = helmetInstance.GetComponent<Rigidbody>();
				if (helmetRigidbody != null)
				{
					Vector3 randomDirection = Random.onUnitSphere;
					float randomForce = Random.Range(minForce, maxForce);
					helmetRigidbody.AddForce(randomDirection * randomForce, ForceMode.Impulse);
				}
			}
		}

		Instantiate(deathParticles, transform.position, Quaternion.identity);

		//Sound
		playerController.playerAudioManager.PlayDeath();

		//Ghost
		ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
		ghost.GetComponent<GhostInputHandler>().InitializeGhost(playerConfig);

		//Power control pass
		currentPower = powerController.GetCurrentPowerLevel() / 2;
		if (lastAttacker != null) lastAttacker.GetComponent<PowerController>().AddPowerLevel(currentPower); //Se le suma la puntuacion del enemigo
		powerController.OnDieSetCurrentPowerLevel();

		//Logic
		dead = true;
		DisablePlayer();
	}

	private void DisablePlayer()
	{
		playerController.enabled = false;
		powerController.enabled = false;
		playerHudController.DisableHud();
		playersHealthManager.NotifyDead(this.gameObject.transform);
	}

	private void RestoreHealth(float healthAmmount)
	{
		float totalHealth = maxHealth * healthAmmount;
		health += totalHealth;
		if (health >= maxHealth) health = maxHealth;
		playerHudController.ChangeUIHealth(health, maxHealth);
	}

	private void HandleCurrentPowerChanged(float newValue)
	{
		maxHealth = maxHealthBase + powerController.PowerHealth();
	}
	#endregion

	#region Collisions and Triggers
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("JumpAttack") && !dead)
		{
			lastAttacker = other.transform.parent.gameObject;
			SlashController slashController = other.GetComponent<SlashController>();
			attackPosition = other.gameObject.transform.position;
			pushBack = true;
			pushForce = slashController.pushForce;
			ReceiveDamage(slashController.finalDamage);
		}

		if (other.CompareTag("Potion"))
		{
			RestoreHealth(other.GetComponent<RestoreHealthEvent>().recoverAmmountMultiplier);
			Destroy(other.gameObject);
		}
		if (other.transform.CompareTag("Projectile") && invencibleTimer <= 0 && !dead)
		{
			DrunkProjectile projectile = other.gameObject.GetComponent<DrunkProjectile>();
			lastAttacker = projectile.owner;
			attackPosition = projectile.owner.transform.position;
			pushBack = true;
			pushForce = projectile.pushForce;
			ReceiveDamage(projectile.finalDamage);
			Debug.Log("Recibiendo daño");
		}
		if (other.CompareTag("EventDamage") && invencibleTimer <= 0 && !dead)
		{
			ReceiveDamageMultiplier(other.GetComponent<DealDamageEvent>().GetDamageMultipler());
		}
		if (other.gameObject.tag == "EnemyCharge" && invencibleTimer <= 0 && !dead)
		{
			Charge charge = other.gameObject.GetComponent<Charge>();
			lastAttacker = charge.owner;
			attackPosition = charge.owner.transform.position;
			pushBack = true;
			pushForce = charge.pushForce;
			ReceiveDamage(charge.finalDamage);
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("SlashEffect") && invencibleTimer <= 0 && !dead)
		{
			SlashController slashController = other.GetComponent<SlashController>();
			lastAttacker = slashController.owner;
			attackPosition = slashController.owner.transform.position;
			pushBack = true;
			pushForce = slashController.pushForce;
			ReceiveDamage(slashController.finalDamage);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Arrow") && invencibleTimer <= 0 && !dead)
		{
			ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
			if (ac)
			{
				if (this.gameObject == ac.owner)
				{
					//Se pega contra si mismo al principio, no hace nada
				}
				else
				{
					lastAttacker = ac.owner;
					attackPosition = ac.ownerPos;
					pushBack = true;
					pushForce = ac.pushForce;

					ReceiveDamage(ac.finalDamage);
					Destroy(collision.gameObject);
				}
			}
			else
			{
				BulletController bc = collision.gameObject.GetComponent<BulletController>();
				if (bc)
				{
					if (this.gameObject == bc.owner)
					{
						//Se pega contra si mismo al principio, no hace nada
					}
					else
					{
						lastAttacker = bc.owner;
						attackPosition = bc.ownerPos;
						pushBack = true;
						pushForce = bc.pushForce;

						ReceiveDamage(bc.finalDamage);
						Destroy(collision.gameObject);
					}
				}
			}
		}

		if (collision.transform.CompareTag("TorusAtk") && invencibleTimer <= 0 && !dead)
		{
			Torus torus = collision.gameObject.GetComponent<Torus>();
			lastAttacker = torus.owner;
			attackPosition = torus.owner.transform.position;
			pushBack = true;
			pushForce = torus.pushForce;
			ReceiveDamage(torus.finalDamage);
		}

		if (collision.transform.CompareTag("Stone") && invencibleTimer <= 0 && !dead)
		{
			RockEvent rock = collision.gameObject.GetComponent<RockEvent>();
			lastAttacker = null;
			attackPosition = rock.originalPosition;
			pushBack = true;
			pushForce = rock.pushForce;
			ReceiveDamage(rock.damage);
		}
	}
	#endregion

	#region Coroutines
	IEnumerator ScaleUpAndDown(Transform transform, Vector3 upScale, float duration)
	{
		Vector3 initialScale = transform.localScale;

		for (float time = 0; time < duration * 2; time += Time.deltaTime)
		{
			transform.localScale = Vector3.Lerp(initialScale, upScale, time);
			yield return null;
		}

		//anim.SetTrigger("Respawn");
	}

	IEnumerator RedEffect()
	{
		int numTimes = 5;
		float delay = 0.1f;
		for (int i = 0; i < numTimes; i++)
		{
			helmet.material = redMaterial;
			body.material = redMaterial;
			yield return new WaitForSeconds(delay);
			helmet.material = originalHelmetMaterial;
			body.material = originalBodyMaterial;
			yield return new WaitForSeconds(delay);
		}
	}
	#endregion
}
