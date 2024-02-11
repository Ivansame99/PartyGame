using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerHealthController : MonoBehaviour
{
	[Header("Logic")]
	private float health;

	[SerializeField]
	private float maxHealth;

	[SerializeField]
	private float inmuneTime;

	[HideInInspector]
	public bool dead = false;
	private bool deadAux = false;

	private GameObject lastAttacker;
	private float currentPower;

	private PlayersRespawn playersRespawn;

	private bool pushBack;
	private Vector3 attackPosition;
	private float pushForce;

	[HideInInspector]
	public float invencibleTimer;

	[Header("UI")]
	[SerializeField]
	private HealthBarController healthBarC;

	[SerializeField]
	private Transform healthBar;

	private Animator healthBarAnimator;

	[SerializeField]
	private Transform powerBar;

	private Canvas healBarCanvas;

	private Camera camera;

	private GameObject playerUI;
	private HealthBarController playerUIHealth;
	private Animator playerUIHealthAnimator;

	[Header("Components")]
	private PlayerController playerController;
	private PowerController powerController;
	private Animator anim;
	private Rigidbody rb;

	public Material URPMaterial;
	public Texture baseMapParpadeo;
	public Texture baseMapOriginal;

	[SerializeField]
	private AudioSource hitSound;
	[SerializeField] private float minPitch;
	[SerializeField] private float maxPitch;

	[Header("Feedback")]
	[SerializeField] private GameObject cross1;
	[SerializeField] private GameObject cross2;
	[SerializeField] private GameObject glow;

	[SerializeField] private GameObject HealParticles;
	[SerializeField] private GameObject DeathParticles;
	[SerializeField] private GameObject BloodParticles;
	[SerializeField] private GameObject skullsBounds;

	void Start()
	{
		healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
		camera = Camera.main;
		SetupHealthBar(healBarCanvas, camera);
		health = maxHealth;

		//Components
		playerController = GetComponent<PlayerController>();
		powerController = GetComponent<PowerController>();
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		playersRespawn = FindObjectOfType<PlayersRespawn>();

		URPMaterial.SetTexture("_BaseMap", baseMapOriginal);
		healthBarAnimator = healthBar.gameObject.GetComponent<Animator>();
		playerUI = GameObject.FindGameObjectWithTag("UI" + this.gameObject.name);
		playerUIHealthAnimator = playerUI.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
		playerUIHealth = playerUI.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<HealthBarController>();
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
			StartCoroutine(ScaleUpAndDown(this.transform, new Vector3(0f, 0f, 0f), 1f));
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

	public void ReceiveDamage(float damage)
	{
		//Feedback
		StartCoroutine(RedEffect());
		Instantiate(BloodParticles, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

		cross1.SetActive(false);
		cross2.SetActive(false);
		glow.SetActive(false);

		cross1.SetActive(true);
		cross2.SetActive(true);
		glow.SetActive(true);

		//Sound
		hitSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		hitSound.Play();

		//Animations
		healthBarAnimator.SetTrigger("Damage");
		playerUIHealthAnimator.SetTrigger("Damage");

		//Logic
		health -= damage;

		if (healthBarC != null)
		{
			ChangeUI();
		}

		if (health <= 0) Die();

		invencibleTimer = inmuneTime;
	}

	void RestoreHealth(float healthAmmount)
	{
		health += healthAmmount;
		if (health >= maxHealth) health = maxHealth;
		ChangeUI();
	}

	private void Die()
	{
		//Feedback
		Instantiate(DeathParticles, transform.position, Quaternion.identity);
		Instantiate(skullsBounds, transform.position, Quaternion.identity);
		anim.SetTrigger("Death");

		//Power control pass
		currentPower = GetComponent<PowerController>().GetCurrentPowerLevel() / 2;
		if (lastAttacker != null) lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower); //Se le suma la puntuacion del enemigo
		GetComponent<PowerController>().OnDieSetCurrentPowerLevel();

		//Logic
		playersRespawn.NotifyDead(this.gameObject.transform);
		dead = true;
		DisablePlayer();
	}

	private void DisablePlayer()
	{
		playerController.enabled = false;
		powerController.enabled = false;
		healthBar.gameObject.SetActive(false);
		powerBar.gameObject.SetActive(false);
	}

	private void SetupHealthBar(Canvas canvas, Camera camera)
	{
		healthBar.transform.SetParent(canvas.transform);
	}

	void ChangeUI()
	{
		healthBarC.SetProgress(health / maxHealth, 2);
		playerUIHealth.SetProgress(health / maxHealth, 2);
	}

	public void EnablePlayer()
	{
		powerController.enabled = true;
		playerController.enabled = true;
		healthBar.gameObject.SetActive(true);
		powerBar.gameObject.SetActive(true);
		dead = false;
		deadAux = false;
		health = maxHealth;
		invencibleTimer = 0.5f;
		ChangeUI();
	}

	IEnumerator ScaleUpAndDown(Transform transform, Vector3 upScale, float duration)
	{
		Vector3 initialScale = transform.localScale;

		for (float time = 0; time < duration * 2; time += Time.deltaTime)
		{
			transform.localScale = Vector3.Lerp(initialScale, upScale, time);
			yield return null;
		}

		anim.SetTrigger("Respawn");
	}

	IEnumerator RedEffect()
	{
		for (int i = 0; i < 5; i++)
		{
			URPMaterial.SetTexture("_BaseMap", baseMapParpadeo);
			yield return new WaitForSeconds(0.1f);
			URPMaterial.SetTexture("_BaseMap", baseMapOriginal);
			yield return new WaitForSeconds(0.1f);
		}
	}

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
			RestoreHealth(other.GetComponent<RestoreHealthEvent>().recoverAmmount);
			Destroy(other.gameObject);
		}

		if (other.CompareTag("EventDamage"))
		{
			ReceiveDamage(other.GetComponent<DealDamageEvent>().damageAmmount);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("SlashEffect") && invencibleTimer <= 0 && !dead)
		{
			lastAttacker = other.transform.parent.gameObject;
			SlashController slashController = other.GetComponent<SlashController>();
			attackPosition = other.gameObject.transform.position;
			pushBack = true;
			pushForce = slashController.pushForce;
			ReceiveDamage(slashController.finalDamage);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Arrow" && invencibleTimer <= 0 && !dead)
		{
			ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
			if (this.gameObject == ac.owner && ac.invencibilityTimerOnSpawnOwner > 0)
			{
				//Se pega contra si mismo al principio, no hace nada
			}
			else
			{
				lastAttacker = ac.owner;
				attackPosition = collision.gameObject.transform.position;
				pushBack = true;
				pushForce = ac.pushForce;

				ReceiveDamage(ac.finalDamage);
				Destroy(collision.gameObject);
			}
		}
	}
}
