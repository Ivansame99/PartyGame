using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drunk : Enemy
{
    [Header("Enemy States")]
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private EnemySpecialAttackSOBase enemySpecialAttackBase;

    void Awake()
    {
        //Initialize SO
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        enemySpecialAttackBaseInstance = Instantiate(enemySpecialAttackBase);

        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        specialAttackState = new EnemySpecialAttackState(this, stateMachine);
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
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);
        enemySpecialAttackBaseInstance.Init(gameObject, this);

        //Initialize State Machine
        stateMachine.Initialize(chaseState);
    }
}
