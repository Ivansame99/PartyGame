using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] private float normalAttackSpeed;
    [SerializeField] private float specialAttackSpeed;
    [SerializeField] private float enemyBaseDamage;
    [SerializeField] private float enemyBasePushForce;
    [SerializeField] private float damagePushForce;
    private Rigidbody rb;
    private Animator animator;
    private EnemyTarget enemyTarget;
    private bool onlyOnceSpecial,onlyOnceAttack,onlyOnceDamaged;
    NavMeshAgent agent;

    //SLASH STUFF
    private PowerController powerController;

    [SerializeField] 
    private GameObject bigSlashParticleSystem;

    private SlashController slashControllerBig;

    [SerializeField]
    private GameObject slashCollider;
    [SerializeField]
    private GameObject bigSlashCollider;
    public SlashController slashController;

    private bool surroundFlag;

    private Vector3 evadeAttackDirection = Vector3.zero;
    private Transform playerPos;
    [SerializeField] private float tooClose;

    private float timer;
    private int xd;

    public GameObject redRectangle;

    // Start is called before the first frame update
    void Start()
    {
        redRectangle.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        evadeAttackDirection = -transform.forward;
        onlyOnceSpecial = true;
        onlyOnceAttack = true;
        slashController = slashCollider.GetComponent<SlashController>();
        slashControllerBig = bigSlashCollider.GetComponent<SlashController>();
        powerController = GetComponent<PowerController>();
        enemyTarget = GetComponent<EnemyTarget>();
        xd = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = enemyTarget.player;
        timer += Time.deltaTime;
        if(timer >= 5)
        {
            xd = Random.Range(0, 2);
            timer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (animator.GetBool("attackOn"))
        {
            agent.enabled = false;
            rb.MovePosition(transform.position + transform.forward * normalAttackSpeed * Time.fixedDeltaTime);
            onlyOnceAttack = false;
            surroundFlag = false;
        }
        
        if (!animator.GetBool("attackOn") && !onlyOnceAttack)
        {
            agent.enabled = true;
            onlyOnceAttack = true;
            //slashCollider.SetActive(false);

            animator.SetTrigger("attackFinished");
        }
        
        if (animator.GetBool("isEvading"))
        {
            //navMeshAgent.enabled = false;
            //METE SLASH ESPECIAL
            bigSlashParticleSystem.SetActive(true);
            bigSlashCollider.SetActive(true);
            slashControllerBig.finalDamage = enemyBaseDamage/3 + powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
            slashControllerBig.pushForce = enemyBasePushForce;
            //rb.MovePosition(transform.position + evadeAttackDirection * normalAttackSpeed * Time.fixedDeltaTime);
            onlyOnceSpecial = false;
        }
        if (!animator.GetBool("isEvading") && !onlyOnceSpecial)
        {
            bigSlashParticleSystem.SetActive(false);
            bigSlashCollider.SetActive(false);
            onlyOnceSpecial = true;
        }

        if (animator.GetBool("isDamaged"))
        {
            agent.enabled = false;
            rb.MovePosition(transform.position + -transform.forward * damagePushForce * Time.fixedDeltaTime);
            onlyOnceDamaged = false;
            //navMeshAgent.isStopped = true;
        }

        if (!animator.GetBool("isDamaged") && !onlyOnceDamaged)
        {
            agent.enabled = true;
            onlyOnceDamaged = true;
        }
        /*
        if (animator.GetBool("isSurrounding"))
        {
            agent.enabled = false;
            float distance = Vector3.Distance(transform.position, enemyTarget.player.position);

            if(distance > 8) rb.MovePosition(transform.position + playerPos.forward * 1f * Time.fixedDeltaTime);
            else if(distance <8) rb.MovePosition(transform.position + -playerPos.forward * 1f * Time.fixedDeltaTime);
            //SI ESTA DEMASIADO LEJOS
            if(xd == 0) rb.MovePosition(transform.position + playerPos.right * 2f * Time.fixedDeltaTime);
            else if(xd == 1) rb.MovePosition(transform.position + -playerPos.right * 2f * Time.fixedDeltaTime);
        }
        if (!animator.GetBool("isSurrounding") && !surroundFlag)
        {
            agent.enabled = true;
            surroundFlag = true;
        }
        */
    }

    public void Slash()
    {
        slashController.finalDamage = enemyBaseDamage + powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
        slashController.pushForce = enemyBasePushForce;
    }

}
