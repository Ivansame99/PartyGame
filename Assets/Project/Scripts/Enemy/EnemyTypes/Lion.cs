using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Lion : Enemy
{
    [Header("Enemy States")]
    [SerializeField] private EnemyIdleSOBase enemyIdleBase;
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyDeathSOBase enemyDeathBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private BossTorusSOBase bossTorusAttack;
    [SerializeField] private BossDistanceAttackSOBase bossDistanceAttack;
    [SerializeField] private EnemySpecialAttackSOBase enemySpecialAttackBase;

    private void Awake()
    {
        enemyIdleBaseInstance = Instantiate(enemyIdleBase);
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyDeathBaseInstance = Instantiate(enemyDeathBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        bossTorusBaseInstance = Instantiate(bossTorusAttack);
        bossDistanceAttackBaseInstance = Instantiate(bossDistanceAttack);
        enemySpecialAttackBaseInstance = Instantiate(enemySpecialAttackBase);

        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        deathState = new EnemyDeathState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        bossTorusState = new BossTorus(this, stateMachine);
        bossDistanceAttackState = new BossDistanceAttack(this, stateMachine);
        specialAttackState = new EnemySpecialAttackState(this, stateMachine);

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
        enemyDirector = GameManager.Instance.enemyDirector;

        //Initialize SO
        enemyIdleBaseInstance.Init(gameObject, this);
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyDeathBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);
        bossTorusBaseInstance.Init(gameObject, this);
        bossDistanceAttackBaseInstance.Init(gameObject, this);
        enemySpecialAttackBaseInstance.Init(gameObject, this);

        //Initialize State Machine
        stateMachine.Initialize(idleState);
    }
}
