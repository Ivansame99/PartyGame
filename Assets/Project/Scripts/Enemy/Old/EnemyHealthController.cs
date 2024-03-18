using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

[System.Serializable]
public class HelmetPrefab
{
    public GameObject prefab;
    [Range(0, 1)]
    public float spawnChance;
}


public class EnemyHealthController : MonoBehaviour
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

    private bool pushBack;
    private Vector3 attackPosition;
    private float pushForce;

    private Animator animator;

    public bool invencibility=false;
    [SerializeField] private bool damageAnim;

    public bool dead = false;

    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;


    [SerializeField] private GameObject DeathParticles;
    [SerializeField] private GameObject BloodParticles;
	[SerializeField] private GameObject floatingDamageText;

	//[SerializeField] private GameObject Helmet1;
	//[SerializeField] private GameObject Helmet2;
	// [SerializeField] private GameObject Helmet3;
	// [SerializeField] private GameObject Helmet4;
	// [SerializeField] private GameObject Helmet5;
	public HelmetPrefab[] helmetPrefabs;
    private PowerController powerController;
    public float minForce = 1f;
    public float maxForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
		powerController=GetComponent<PowerController>();
		healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupHealthBar(healBarCanvas, cameraMain);
        animator = GetComponent<Animator>();
		maxHealth = maxHealthBase + powerController.PowerHealth();
		if (powerController != null)
		{
			powerController.OnCurrentPowerChanged += HandleCurrentPowerChanged;
		}
		health = maxHealth;
	}

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            invencibility = true;
            timer -= Time.deltaTime;
        } else invencibility = false;
    }

    private void FixedUpdate()
    {
        if (pushBack)
        {
            Vector3 direction = (this.transform.position - attackPosition).normalized;
            direction.y = 0;
            this.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForce, ForceMode.Impulse);
            pushBack = false;
        }
    }

    public void ReceiveDamageSlash(float damage)
    {
        //Feedback
        Instantiate(BloodParticles, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
		Cross1.SetActive(false);
		Cross2.SetActive(false);
		Glow.SetActive(false);


		Cross1.SetActive(true);
		Cross2.SetActive(true);
		Glow.SetActive(true);

		if (floatingDamageText != null) ShowDamageText(damage);

		//Logic
		health -= damage;
        timer = inmuneTime;
        if (health <= 0)
        {
            Die();
            deathSound.pitch = UnityEngine.Random.Range(1.2f, 1.5f);
            deathSound.Play();
        }
        if (healthBarC != null)
        {
            healthBarC.SetProgress(health / maxHealth, 5f);
            if (health >= 0 && !damageAnim && !animator.GetBool("isEvading"))
            {
                animator.SetTrigger("damage");
                hitSound.pitch = UnityEngine.Random.Range(1f, 1.2f);
                hitSound.Play();
            }
        }
    }

    public void ReceiveDamageArrow(float damage)
    {
        health -= damage;

		if (floatingDamageText != null) ShowDamageText(damage);

		if (healthBarC != null)
        {
            healthBarC.SetProgress(health / maxHealth, 5f);
            if (health >= 0 && damageAnim && !animator.GetBool("isEvading"))
            {
                animator.SetTrigger("damage");
                hitSound.pitch = UnityEngine.Random.Range(1f, 1.2f);
                hitSound.Play();
            }
        }
        if (health <= 0)
        {
            Die();
            deathSound.pitch = UnityEngine.Random.Range(1.2f, 1.5f);
            deathSound.Play();
        }
    }

	void ShowDamageText(float damage)
	{
		TMP_Text text = Instantiate(floatingDamageText, transform.position, Quaternion.identity).GetComponent<TMP_Text>();
		text.text = ((int)damage).ToString();
	}

	void Die()
    {
        animator.SetTrigger("die");
       
        /*float destroyDelay = Random.value;
        Destroy(this.gameObject, destroyDelay);
        Destroy(healthBar.gameObject, destroyDelay);*/
        currentPower = GetComponent<PowerController>().GetCurrentPowerLevel();
        if(lastAttacker!=null) lastAttacker.GetComponent<PowerController>().AddPowerLevel(currentPower / 2); //Se le suma la puntuacion del enemigo
        dead = true;
        Invoke("enemyDestroy", 2.0f);
    }

    public void enemyDestroy()
    {
        // Instantiate(Helmet1, transform.position, Quaternion.identity);
        //Instantiate(Helmet2, transform.position, Quaternion.identity);
        // Instantiate(Helmet3, transform.position, Quaternion.identity);
        // Instantiate(Helmet4, transform.position, Quaternion.identity);
        // Instantiate(Helmet5, transform.position, Quaternion.identity);

        foreach (var helmetPrefab in helmetPrefabs)
        {
            if (Random.value <= helmetPrefab.spawnChance)
            {
                // Desplaza ligeramente la posici�n de origen del casco
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 0.5f;
                GameObject helmetInstance = Instantiate(helmetPrefab.prefab, spawnPosition, Quaternion.identity);
                Rigidbody helmetRigidbody = helmetInstance.GetComponent<Rigidbody>();
                if (helmetRigidbody != null)
                {
                    // Genera una fuerza aleatoria en una direcci�n aleatoria
                    Vector3 randomDirection = Random.onUnitSphere;
                    float randomForce = Random.Range(minForce, maxForce);
                    helmetRigidbody.AddForce(randomDirection * randomForce, ForceMode.Impulse);
                }
            }
        }


        Instantiate(DeathParticles, transform.position, Quaternion.identity);
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
                SlashController slashController = other.GetComponent<SlashController>();
				lastAttacker = slashController.owner;
				attackPosition = slashController.owner.transform.position;
				pushBack = true;
                pushForce = slashController.pushForce;

                ReceiveDamageSlash(slashController.finalDamage);
            }
        }

		if (other.CompareTag("EventDamage"))
		{
			ReceiveDamageSlash(other.GetComponent<DealDamageEvent>().damageAmmount);
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow" && !invencibility && !dead)
        {
            ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
            attackPosition = ac.ownerPos;
            pushBack = true;
            pushForce = ac.pushForce;
            lastAttacker = ac.owner;
            ReceiveDamageArrow(ac.finalDamage);
            Destroy(collision.gameObject);
        }
    }
}
