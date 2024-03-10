using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drunk : Enemy
{
    [Header("Enemy States")]
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;

    void Awake()
    {
        //Initialize SO
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);

        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        chaseState = new EnemyChaseState(this, stateMachine);
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

        //Initialize State Machine
        stateMachine.Initialize(chaseState);
    }
}
