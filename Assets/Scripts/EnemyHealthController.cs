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

    private float timer;

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

    // Start is called before the first frame update
    void Start()
    {
        healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupHealthBar(healBarCanvas, camera);
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
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
        if (timer <= 0)
        {
            //Debug.Log(damage);
            health -= damage;
            timer = inmuneTime;
            healthBarC.SetProgress(health / maxHealth, 5f);
            if (health <= 0) Die();
        }
    }

    public void ReceiveDamageArrow(float damage)
    {
        health -= damage;
        healthBarC.SetProgress(health / maxHealth, 5f);
        if (health <= 0) Die();
    }

    void Die()
    {
        /*float destroyDelay = Random.value;
        Destroy(this.gameObject, destroyDelay);
        Destroy(healthBar.gameObject, destroyDelay);*/
        currentPower = GetComponent<PowerController>().GetCurrentPowerLevel();
        lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower / 2); //Se le suma la puntuacion del enemigo
        Destroy(healthBar.gameObject);
        Destroy(powerLevelGameObject.gameObject);
        Destroy(this.gameObject);

    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        healthBar.transform.SetParent(canvas.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlashEffect"))
        {
            Cross1.SetActive(false);
            Cross2.SetActive(false);
            Glow.SetActive(false);


            Cross1.SetActive(true);
            Cross2.SetActive(true);
            Glow.SetActive(true);

            SlashController slashController = other.GetComponent<SlashController>();
            attackPosition = other.gameObject.transform.position;
            pushBack = true;
            pushForce = slashController.pushForce;

            ReceiveDamageSlash(slashController.finalDamage);
            lastAttacker = other.transform.parent.parent.gameObject;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
            attackPosition = collision.gameObject.transform.position;
            pushBack = true;
            pushForce = ac.pushForce;

            ReceiveDamageArrow(ac.finalDamage);
            lastAttacker = ac.owner;
            Destroy(collision.gameObject);
        }
    }
}
