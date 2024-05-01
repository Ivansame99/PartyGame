using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Giant : Enemy
{
    [Header("Enemy States")]
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private EnemySpecialAttackSOBase enemySpecialAttackBase;
    [SerializeField] private EnemyDeathSOBase enemyDeathBase;
    [SerializeField] private EnemyWaterChaseSOBase enemyWaterChaseBase;
    [SerializeField] private EnemyWaterAttackSOBase enemyWaterAttackBase;

    [SerializeField] private EnemyHealth enemyHealth;

    void Awake()
    {
        //Initialize SO
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        enemySpecialAttackBaseInstance = Instantiate(enemySpecialAttackBase);
        enemyDeathBaseInstance = Instantiate(enemyDeathBase);
        enemyWaterChaseBaseInstance = Instantiate(enemyWaterChaseBase);
        enemyWaterAttackBaseInstance = Instantiate(enemyWaterAttackBase);

        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        specialAttackState = new EnemySpecialAttackState(this, stateMachine);
        deathState = new EnemyDeathState(this, stateMachine);
        waterChaseState = new EnemyWaterChaseState(this, stateMachine);
        waterAttackState = new EnemyWaterAttackState(this, stateMachine);
    }
    void Start()
    {
        //Initialize Health
        //currentHealth = maxHealth;

        //Get Components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        powerController = GetComponent<PowerController>();
        enemyTargetController = GetComponent<EnemyTargetController>();
		giantAudioManager = GetComponent<GiantAudioManager>();
        enemyHealthController = enemyHealth;

		//Initialize SO
		enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);
        enemySpecialAttackBaseInstance.Init(gameObject, this);
        enemyDeathBaseInstance.Init(gameObject, this);
        enemyWaterChaseBaseInstance.Init(gameObject, this);

        //Initialize State Machine
        stateMachine.Initialize(chaseState);
    }
}
