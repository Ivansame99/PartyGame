using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PracticeDummyHealthController : MonoBehaviour
{
	private float health;

	[SerializeField]
	private float maxHealthBase;

	private float maxHealth;

	[SerializeField]
	private float inmuneTime;

	[SerializeField]
	private HealthBarController healthBarC;

	[SerializeField]
	private Transform healthBar;

	public float timer;

	//Variables que iran donde se spawneen los pjs
	private Canvas healBarCanvas;

	[SerializeField]
	private Camera cameraMain;

	[SerializeField]
	private GameObject powerLevelGameObject;
	private float currentPower;

	private GameObject lastAttacker;
	[SerializeField]
	private GameObject Cross1, Cross2, Glow;

	private Vector3 attackPosition;

	public bool invencibility = false;

	public bool dead = false;

	private PowerController powerController;

	private Animator anim;

	[SerializeField] private GameObject deathParticles;
	[SerializeField] private GameObject floatingDamageText;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}
	// Start is called before the first frame update
	void Start()
	{
		powerController = GetComponent<PowerController>();
		healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
		SetupHealthBar(healBarCanvas, cameraMain);
		maxHealth = maxHealthBase + powerController.PowerHealth();
		if (powerController != null)
		{
			powerController.OnCurrentPowerChanged += HandleCurrentPowerChanged;
		}
		health = maxHealth;
	}

	public void ReceiveDamageSlash(float damage)
	{
		//Feedback
		Cross1.SetActive(false);
		Cross2.SetActive(false);
		Glow.SetActive(false);


		Cross1.SetActive(true);
		Cross2.SetActive(true);
		Glow.SetActive(true);

		if (floatingDamageText != null) ShowDamageText(damage);

		//anim.SetTrigger("Damage");

		//Logic
		health -= damage;
		timer = inmuneTime;

		if (health <= 0)
		{
			Die();
		}

		if (healthBarC != null)
		{
			healthBarC.SetProgress(health / maxHealth, 5f);
		}
	}

	void ShowDamageText(float damage)
	{
		TMP_Text text = Instantiate(floatingDamageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
		text.text = ((int)damage).ToString();
	}

	public void ReceiveDamageArrow(float damage)
	{
		//anim.SetTrigger("Damage");
		health -= damage;

		if (floatingDamageText != null) ShowDamageText(damage);

		if (healthBarC != null)
		{
			healthBarC.SetProgress(health / maxHealth, 5f);
		}

		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Vector3 particlesPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
		Instantiate(deathParticles, particlesPos, Quaternion.identity);
		currentPower = GetComponent<PowerController>().GetCurrentPowerLevel();
		if (lastAttacker != null) lastAttacker.GetComponent<PowerController>().AddPowerLevel(currentPower / 2); //Se le suma la puntuacion del enemigo
		dead = true;
		EnemyDestroy();
	}

	public void EnemyDestroy()
	{
		Destroy(this.gameObject);
		Destroy(healthBar.gameObject);
		Destroy(powerLevelGameObject.gameObject);
	}

	public void SetupHealthBar(Canvas canvas, Camera camera)
	{
		healthBar.transform.SetParent(canvas.transform);
	}

	private void HandleCurrentPowerChanged(float newValue)
	{
		maxHealth = maxHealthBase + powerController.PowerHealth();
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.CompareTag("SlashEffect") || other.CompareTag("JumpAttack")) && !invencibility && !dead)
		{
			if (other.gameObject.transform.parent.tag != "Enemy")
			{
				lastAttacker = other.transform.parent.gameObject;

				SlashController slashController = other.GetComponent<SlashController>();
				attackPosition = other.gameObject.transform.position;

				ReceiveDamageSlash(slashController.finalDamage);
			}
		}

		//if (other.CompareTag("EventDamage"))
		//{
		//	ReceiveDamageSlash(other.GetComponent<DealDamageEvent>().damageAmmount);
		//}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Arrow") && !invencibility && !dead)
		{
			ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
			if (ac)
			{
				lastAttacker = ac.owner;
				attackPosition = ac.ownerPos;

				ReceiveDamageArrow(ac.finalDamage);
				Destroy(collision.gameObject);
			}
			else
			{
				BulletController bc = collision.gameObject.GetComponent<BulletController>();
				if (bc)
				{
					lastAttacker = bc.owner;
					attackPosition = bc.ownerPos;

					ReceiveDamageArrow(bc.finalDamage);
					Destroy(collision.gameObject);
				}
			}
		}
	}
}
