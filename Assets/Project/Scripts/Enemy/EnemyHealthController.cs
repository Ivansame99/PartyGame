using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class EnemyHealthController : MonoBehaviour
{

    private float health;

    [SerializeField]
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
    private Camera camera;

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

    // Start is called before the first frame update
    void Start()
    {
        healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupHealthBar(healBarCanvas, camera);
        health = maxHealth;
        animator = GetComponent<Animator>();
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
        //Debug.Log(damage);
        Instantiate(BloodParticles, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
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

    void Die()
    {
        animator.SetTrigger("die");
       
        /*float destroyDelay = Random.value;
        Destroy(this.gameObject, destroyDelay);
        Destroy(healthBar.gameObject, destroyDelay);*/
        currentPower = GetComponent<PowerController>().GetCurrentPowerLevel();
        if(lastAttacker!=null) lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower / 2); //Se le suma la puntuacion del enemigo       
        dead = true;
        Invoke("enemyDestroy", 2.0f);
    }

    public void enemyDestroy()
    {
        Instantiate(DeathParticles, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(healthBar.gameObject);
        Destroy(powerLevelGameObject.gameObject);
    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        healthBar.transform.SetParent(canvas.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlashEffect") && !invencibility && !dead)
        {
            if (other.gameObject.transform.parent.tag != "Enemy")
            {
                
                Cross1.SetActive(false);
                Cross2.SetActive(false);
                Glow.SetActive(false);


                Cross1.SetActive(true);
                Cross2.SetActive(true);
                Glow.SetActive(true);

                lastAttacker = other.transform.parent.gameObject;

                SlashController slashController = other.GetComponent<SlashController>();
                attackPosition = other.gameObject.transform.position;
                pushBack = true;
                pushForce = slashController.pushForce;

                ReceiveDamageSlash(slashController.finalDamage);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow" && !invencibility && !dead)
        {
            ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
            attackPosition = collision.gameObject.transform.position;
            pushBack = true;
            pushForce = ac.pushForce;
            lastAttacker = ac.owner;
            ReceiveDamageArrow(ac.finalDamage);
            Destroy(collision.gameObject);
        }
    }
}
