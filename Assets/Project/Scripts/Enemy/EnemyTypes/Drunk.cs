using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drunk : Enemy
{
    [Header("Enemy States")]
    [Header("Movement States")]
    [SerializeField] private EnemyIdleSOBase enemyIdleBase;
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [Header("Attack States")]
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private EnemySpecialAttackSOBase enemySpecialAttackBase;
    [Header("Health States")]
    [SerializeField] private EnemyDamageSOBase enemyDamageBase;
    [SerializeField] private EnemyDeathSOBase enemyDeathBase;

    void Awake()
    {
        //Initialize SO
        enemyIdleBaseInstance = Instantiate(enemyIdleBase);
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        enemySpecialAttackBaseInstance = Instantiate(enemySpecialAttackBase);
        enemyDamageBaseInstance = Instantiate(enemyDamageBase);
        enemyDeathBaseInstance = Instantiate(enemyDeathBase);

        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        //Movement States
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);

        //Attack States
        attackState = new EnemyAttackState(this, stateMachine);
        specialAttackState = new EnemySpecialAttackState(this, stateMachine);

        //Health States
        damageState = new EnemyDamageState(this, stateMachine);
        deathState = new EnemyDeathState(this, stateMachine);
    }

    void Start()
    {
        currentHealth = maxHealth;

        //Get Components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        powerController = GetComponent<PowerController>();
        enemyTargetController = GetComponent<EnemyTargetController>();

        //Initialize SO
        enemyIdleBaseInstance.Init(gameObject, this);
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);
        enemySpecialAttackBaseInstance.Init(gameObject, this);
        enemyDamageBaseInstance.Init(gameObject, this);
        enemyDeathBaseInstance.Init(gameObject, this);

        //Initialize State Machine
        stateMachine.Initialize(chaseState);
    }
}
