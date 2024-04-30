using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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

	[SerializeField] private GameObject hitParticles;

	[SerializeField] private Renderer body;

	[SerializeField] private Material whiteMaterial;

	[SerializeField] private GameObject powerParticlesPrefab;

	private Material originalBodyMaterial;

	private int powerPerParticle;
	private int numberOfPowerParticles;

	private Vector3 scale = new Vector3(1, 1, 1);

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

		originalBodyMaterial = body.materials[1];
	}

	public void ReceiveDamageSlash(float damage)
	{
		DamageFeedback();

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

		DamageFeedback();

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

	void DamageFeedback()
	{
		//Feedback
		Cross1.SetActive(false);
		Cross2.SetActive(false);
		Glow.SetActive(false);

		Instantiate(hitParticles, this.transform.position, Quaternion.identity);

		StartCoroutine(RedEffect());

		Cross1.SetActive(true);
		Cross2.SetActive(true);
		Glow.SetActive(true);

	}

	void CalculateNumberOfParticles()
	{
		int powerToCalculate = this.powerController.GetHalfPowerLevel();

		if (powerToCalculate >= 0 && powerToCalculate <= 10) // ENTRE O Y 20 DE FUERZA
		{
			numberOfPowerParticles = 2;
			scale = new Vector3(0.5f, 0.5f, 0.5f);

		}
		else if (powerToCalculate >= 11 && powerToCalculate <= 25) // ENTRE 20 Y 50
		{
			numberOfPowerParticles = 5;
			scale = new Vector3(0.6f, 0.6f, 0.6f);
		}
		else if (powerToCalculate >= 26 && powerToCalculate <= 50) // ENTRE 50 Y 100
		{
			numberOfPowerParticles = 10;
			scale = new Vector3(0.7f, 0.7f, 0.7f);
		}
		else if (powerToCalculate >= 51 && powerToCalculate <= 150) // ENTRE 100 Y 300
		{
			numberOfPowerParticles = 15;
			scale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		else if (powerToCalculate >= 151 && powerToCalculate <= 500) // ENTRE 300 Y 1000
		{
			numberOfPowerParticles = 20;
			scale = new Vector3(0.9f, 0.9f, 0.9f);

		}
		else if (powerToCalculate >= 501) // MAS DE 1000
		{
			numberOfPowerParticles = 25;
			scale = new Vector3(1f, 1f, 1f);

		}

		if (numberOfPowerParticles != 0) powerPerParticle = powerToCalculate / numberOfPowerParticles; //DIVIDES LA MITAD DEL PODER(LO QUE TIENES QUE REPARTIR) ENTRE EL NUMERO DE PARTICULAS QUE SUELTAN, POR LO QUE CADA PARTICULA TIENE SU PODER
	}

	void Die()
	{
		Vector3 particlesPos = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);
		Instantiate(deathParticles, particlesPos, Quaternion.identity);

		Vector3 spawnPosition = new Vector3(this.transform.position.x, this.transform.position.y + 4f, this.transform.position.z);
		CalculateNumberOfParticles();

		for (int i = 0; i < numberOfPowerParticles; i++)
		{
			Vector3 adjustedSpawnPosition = spawnPosition;
			GameObject powerInstance = Instantiate(powerParticlesPrefab, adjustedSpawnPosition, Quaternion.identity);
			powerInstance.GetComponent<PowerParticleController>().SetPowerAmount(powerPerParticle);
			Rigidbody powerRigidbody = powerInstance.GetComponent<Rigidbody>();

			if (powerRigidbody != null)
			{
				powerInstance.transform.localScale = scale;
				powerRigidbody.AddForce(new Vector3(Random.Range(-0.3f, 0.3f), 0.3f, Random.Range(-0.3f, 0.3f)), ForceMode.Impulse);
			}
		}

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

	IEnumerator RedEffect()
	{
		int numTimes = 3;
		float delay = 0.1f;
		for (int i = 0; i < numTimes; i++)
		{
			body.materials[1] = whiteMaterial;
			yield return new WaitForSeconds(delay);
			body.materials[1] = originalBodyMaterial;
			yield return new WaitForSeconds(delay);
		}
	}
}
