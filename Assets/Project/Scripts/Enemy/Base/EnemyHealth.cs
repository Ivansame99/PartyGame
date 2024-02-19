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
    private bool invencibility;
    private float timer;

    //Health UI
    private Canvas healBarCanvas;
    private Camera cameraHealthBar;

    [SerializeField]
    private HealthBarController healthBarC;
    [SerializeField]
    private Transform healthBar;

    private GameObject lastAttacker;
    private Vector3 attackPosition;
    private bool pushBack;
    private float pushForce;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
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
                Debug.Log("entra");
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
}
