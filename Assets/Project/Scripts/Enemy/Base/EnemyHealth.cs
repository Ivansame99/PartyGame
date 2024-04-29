using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyHealth : MonoBehaviour
{
	//Enemy
	[SerializeField] private Enemy enemy;

	//Inmune time after hit
	[HideInInspector] public bool invencibility;
	[HideInInspector] public float timer;

	//Health UI
	private Canvas healBarCanvas;
	private Camera cameraHealthBar;

	//Health Bar
	[Header("Health Bar")]
	[SerializeField] private HealthBarController healthBarC;
	[SerializeField] private Transform healthBar;

	//Power Level
	[Header("Power Level")]
	[SerializeField]
	private GameObject powerLevelGameObject;
	private float currentPower;
	[SerializeField]
	private float maxHealthBase;

	//Attackers info
	private GameObject lastAttacker;
	private Vector3 attackPosition;
	private bool pushBack;
	private float pushForce;

	//Damage Feedback
	[Header("Damage Feedback")]
	[SerializeField] private GameObject floatingDamageText;
	[SerializeField] private GameObject bloodParticles;
	[SerializeField] private GameObject crossRight, crossLeft;
	[SerializeField] private GameObject glow;
	[SerializeField] private GameObject hitParticles;

	[SerializeField] private Material redMaterial;
	[SerializeField] private Renderer helmet;
	[SerializeField] private Renderer body;

	private Material originalHelmetMaterial;
	private Material originalBodyMaterial;

	private void Start()
	{
		if (enemy == null) enemy = GetComponent<Enemy>();
		healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
		SetupHealthBar(healBarCanvas, GetComponent<Camera>());
		//Invoke(nameof(LoadMaxHealth), 0.1f);
		if (enemy.powerController != null)
		{
			enemy.powerController.OnCurrentPowerChanged += HandleCurrentPowerChanged;
		}

		originalHelmetMaterial = helmet.material;
		originalBodyMaterial = body.material;
	}

	void Update()
	{
		if (timer >= 0)
		{
			invencibility = true;
			timer -= Time.deltaTime;
		}
		else invencibility = false;
	}

	private void FixedUpdate()
	{
		if (pushBack)
		{
			Vector3 direction = (this.transform.position - attackPosition).normalized;
			direction.y = 0;
			enemy.rb.AddForce(direction * pushForce, ForceMode.Impulse);
			pushBack = false;
		}
	}

	private void LoadMaxHealth()
	{
		enemy.maxHealth = maxHealthBase + enemy.powerController.PowerHealth();
	}

	public void ReceiveDamage(float damage)
	{
		//Feedback
		if (floatingDamageText != null) ShowDamageText(damage);

		//Logic
		enemy.currentHealth -= damage;
		timer = enemy.inmuneTime;

		if (healthBarC != null)
		{
			healthBarC.SetProgress(enemy.currentHealth / enemy.maxHealth, 5f);
			DamageFeedback();
			enemy.SetDamagedStatus(true);
		}
		if (enemy.currentHealth <= 0)
		{
			Die();
		}
	}

	public void ReceiveDamageMultiplier(float multiplier)
	{
		//Logic
		float damage = enemy.maxHealth * multiplier;

		enemy.currentHealth -= damage;
		timer = enemy.inmuneTime;

		//Feedback
		if (floatingDamageText != null) ShowDamageText(damage);

		if (healthBarC != null)
		{
			healthBarC.SetProgress(enemy.currentHealth / enemy.maxHealth, 5f);
			DamageFeedback();
			enemy.SetDamagedStatus(true);
		}
		if (enemy.currentHealth <= 0)
		{
			Die();
		}
	}

	void DamageFeedback()
	{
		GameObject bloodEffectInstance = Instantiate(bloodParticles, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
		float duration = 40f;
		Destroy(bloodEffectInstance, duration);

		crossRight.SetActive(false);
		crossLeft.SetActive(false);
		glow.SetActive(false);

		StartCoroutine(RedEffect());
		//StartCoroutine(TimeFreeze());
		
		crossRight.SetActive(true);
		crossLeft.SetActive(true);
		glow.SetActive(true);
		Instantiate(hitParticles, this.transform.position, Quaternion.identity);
	}

	void ShowDamageText(float damage)
	{
		TMP_Text text = Instantiate(floatingDamageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
		text.text = ((int)damage).ToString();
	}

	private void HandleCurrentPowerChanged(float newValue)
	{
		enemy.maxHealth = maxHealthBase + enemy.powerController.PowerHealth();
		if (enemy.currentHealth == 0) enemy.currentHealth = enemy.maxHealth; //First time when enemy is instanciated
	}

	void Die()
	{
		//currentPower = enemy.GetPowerDamage();
		//if (lastAttacker != null) lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower / 2); //Se le suma la puntuacion del enemigo
		currentPower = enemy.GetPoweLevel() / 2;
		if (lastAttacker != null) lastAttacker.GetComponent<PowerController>().AddPowerLevel(currentPower);

		enemyDestroy();
	}

	//private void HandleCurrentPowerChanged(float newValue)
	//{
	//	maxHealth = maxHealthBase + powerController.PowerHealth();
	//}

	public void enemyDestroy()
	{
		enemy.isDead = true;
		if (healthBar.gameObject != null) Destroy(healthBar.gameObject);
		Destroy(powerLevelGameObject.gameObject);
	}

	public void SetupHealthBar(Canvas canvas, Camera camera)
	{
		healthBar.transform.SetParent(canvas.transform);
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("SlashEffect") || other.CompareTag("JumpAttack")) && !invencibility && !enemy.isDead)
		{
			if (other.gameObject.transform.parent.tag != "Enemy")
			{
				lastAttacker = other.transform.parent.gameObject;
				SlashController slashController = other.GetComponent<SlashController>();
				attackPosition = other.gameObject.transform.position;
				if (other.CompareTag("JumpAttack")) pushForce = slashController.pushForce;
				else pushForce = slashController.pushForce * 2;
				pushBack = true;

				ReceiveDamage(slashController.finalDamage);
			}
		}
        if (other.transform.CompareTag("Arrow") && !invencibility && !enemy.isDead)
        {
            ArrowController ac = other.gameObject.GetComponent<ArrowController>();
            if (ac)
            {
                lastAttacker = ac.owner;
                attackPosition = ac.ownerPos;
                pushBack = true;
                pushForce = ac.pushForce;

                ReceiveDamage(ac.finalDamage);
                Destroy(other.gameObject);
            }
            else
            {
                BulletController bc = other.gameObject.GetComponent<BulletController>();
                lastAttacker = bc.owner;
                attackPosition = bc.ownerPos;
                pushBack = true;
                pushForce = bc.pushForce;

                ReceiveDamage(bc.finalDamage);
                Destroy(other.gameObject);
            }
        }
        if (other.CompareTag("EventDamage") && !invencibility && !enemy.isDead)
		{
			ReceiveDamageMultiplier(other.GetComponent<DealDamageEvent>().GetDamageMultipler());
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Arrow") && !invencibility && !enemy.isDead)
		{
			ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
			if (ac)
			{
				lastAttacker = ac.owner;
				attackPosition = ac.ownerPos;
				pushBack = true;
				pushForce = ac.pushForce;

				ReceiveDamage(ac.finalDamage);
				Destroy(collision.gameObject);
			}
			else
			{
				BulletController bc = collision.gameObject.GetComponent<BulletController>();
				lastAttacker = bc.owner;
				attackPosition = bc.ownerPos;
				pushBack = true;
				pushForce = bc.pushForce;

				ReceiveDamage(bc.finalDamage);
				Destroy(collision.gameObject);
			}
		}

		if (collision.transform.CompareTag("Stone") && !invencibility && !enemy.isDead)
		{
			RockEvent rock = collision.gameObject.GetComponent<RockEvent>();
			lastAttacker = null;
			attackPosition = rock.originalPosition;
			pushBack = true;
			pushForce = rock.pushForce;
			ReceiveDamage(rock.damage);
		}
	}

	IEnumerator RedEffect()
	{
		float delay = 0.25f;
		helmet.material = redMaterial;
		body.material = redMaterial;
		yield return new WaitForSeconds(delay);
		helmet.material = originalHelmetMaterial;
		body.material = originalBodyMaterial;
	}

	//IEnumerator TimeFreeze()
	//{
	//	float duration = 0.03f;

	//	Time.timeScale = 0;
	//	yield return new WaitForSecondsRealtime(duration);
	//	Time.timeScale = 1;
	//}
}
