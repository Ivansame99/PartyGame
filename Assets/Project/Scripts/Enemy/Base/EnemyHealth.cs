using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //Enemy
    private Enemy enemy;

    //Health
    private float health;
    private float maxHealth;

    //Inmune time
    public bool invencibility;
    public float timer;

    //Health UI
    private Canvas healBarCanvas;
    private Camera cameraHealthBar;

    [SerializeField]
    private HealthBarController healthBarC;
    [SerializeField]
    private Transform healthBar;

    [SerializeField]
    private GameObject powerLevelGameObject;
    private float currentPower;

    private GameObject lastAttacker;
    private Vector3 attackPosition;
    private bool pushBack;
    private float pushForce;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupHealthBar(healBarCanvas, GetComponent<Camera>());
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

    public void ReceiveDamageSlash(float damage)
    {
        //Logic
        enemy.currentHealth -= damage;
        timer = enemy.inmuneTime;

        if (healthBarC != null)
        {
            healthBarC.SetProgress(enemy.currentHealth / enemy.maxHealth, 5f);
        }
        if(enemy.currentHealth <= 0)
        {
            Die();
        }
    }

    public void ReceiveDamageArrow(float damage)
    {
        enemy.currentHealth -= damage;
        if (healthBarC != null)
        {
            healthBarC.SetProgress(health / maxHealth, 5f);
        }
        if (enemy.currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        currentPower = GetComponent<PowerController>().GetCurrentPowerLevel();
        if (lastAttacker != null) lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower / 2); //Se le suma la puntuacion del enemigo       
        enemy.isDead = true;
        Invoke("enemyDestroy", 2.0f);
    }
    public void enemyDestroy()
    {
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
        if ((other.CompareTag("SlashEffect") || other.CompareTag("JumpAttack")) && !invencibility && !enemy.isDead)
        {
            if (other.gameObject.transform.parent.tag != "Enemy")
            {
                lastAttacker = other.transform.parent.gameObject;
                SlashController slashController = other.GetComponent<SlashController>();
                attackPosition = other.gameObject.transform.position;
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
        if (collision.gameObject.tag == "Arrow" && !invencibility && !enemy.isDead)
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
