using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lion : Enemy
{
    [Header("Enemy States")]
    [SerializeField] private EnemyIdleSOBase enemyIdleBase;

    private void Awake()
    {
        enemyIdleBaseInstance = Instantiate(enemyIdleBase);

        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine);
    }
    // Start is called before the first frame update
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

        //Initialize State Machine
        stateMachine.Initialize(idleState);
    }
}
