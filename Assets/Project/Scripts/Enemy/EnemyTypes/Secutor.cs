using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Secutor : Enemy
{
	//public GameObject stunParticles;

	[Header("Enemy States")]
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyPreAttackSOBase enemyPreAttackBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private EnemySpecialAttackSOBase enemySpecialAttackBase;
    [SerializeField] private EnemyStunedSOBase enemyStunedBase;
    [SerializeField] private EnemyDeathSOBase enemyDeathBase;
    [SerializeField] private EnemyDamageSOBase enemyDamageBase;
    void Awake()
    {
        //Initialize SO
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyPreAttackBaseInstance = Instantiate(enemyPreAttackBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        enemySpecialAttackBaseInstance = Instantiate(enemySpecialAttackBase);
        enemyStunedBaseInstance = Instantiate(enemyStunedBase);
        enemyDeathBaseInstance = Instantiate(enemyDeathBase);
        enemyDamageBaseInstance = Instantiate(enemyDamageBase);
        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        chaseState = new EnemyChaseState(this, stateMachine);
        preAttackState = new EnemyPreAttackState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        specialAttackState = new EnemySpecialAttackState(this, stateMachine);
        stunnedState = new EnemyStunnedState(this, stateMachine);
        deathState = new EnemyDeathState(this, stateMachine);
        damageState = new EnemyDamageState(this, stateMachine);
    }
    void Start()
    {
        //Initialize Health
        //currentHealth = maxHealth;

        //Get Components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyHealthController = GetComponent<EnemyHealth>();
        powerController = GetComponent<PowerController>();
        enemyTargetController = GetComponent<EnemyTargetController>();

        //Initialize SO
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyPreAttackBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);
        enemySpecialAttackBaseInstance.Init(gameObject, this);
        enemyStunedBaseInstance.Init(gameObject, this);
        enemyDeathBaseInstance.Init(gameObject, this);
        enemyDamageBaseInstance.Init(gameObject, this);
        //Initialize State Machine
        stateMachine.Initialize(chaseState);
    }
}
