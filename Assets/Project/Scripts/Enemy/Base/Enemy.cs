using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable,ITriggerCheckeable,ICombat
{
    //HEALTH INTERFACE
    [field: Header("Health parameters")]
    [field: SerializeField] public float maxHealth { get; set; } = 100f;
    [field: SerializeField] public float inmuneTime { get; set; }
    public float currentHealth { get; set; }
    public bool isDead { get; set; }


    //MOVEMENT INTERFACE
    public NavMeshAgent agent { get; set; }
    public Rigidbody rb { get; set; }
    public bool state { get; set; }
    public Transform playerPos { get; set; }
    public Transform playerPos2 { get; set; }
    public EnemyTarget enemyTarget { get; set; }
    public Animator animator { get; set; }

    //AGGRO INTERFACE
    public bool IsAggreed { get; set; }
    public bool IsSpecialAggro { get; set; }
    public bool IsImpact { get; set; }

    //COMBAT INTERFICE
    public PowerController powerController { get; set; }
    [field: SerializeField] public SlashController slashController { get; set; }

    #region State Machine Variables
    public EnemyStateMachine stateMachine { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyAttackState attackState { get; set; }
    public EnemySpecialAttackState specialAttackState { get; set; }
    #endregion

    #region SO Variables
    [Header("Enemy States")]
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    [SerializeField] private EnemySpecialAttackSOBase enemySpecialAttackBase;

    public EnemyChaseSOBase enemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase enemyAttackBaseInstance { get; set; }
    public EnemySpecialAttackSOBase enemySpecialAttackBaseInstance { get; set; }


    #endregion

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
        //Initialize Health
        currentHealth = maxHealth;

        //Get Components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        powerController = GetComponent<PowerController>();
        //Initialize SO
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);
        enemySpecialAttackBaseInstance.Init(gameObject, this);

        //Initialize State Machine
        stateMachine.Initialize(chaseState);        
    }
    #region Health/Damage
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Movement Functions
    public void MoveEnemy(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void AgentState(bool state)
    {
        agent.enabled = state;
    }
    #endregion

    #region Animation Triggers
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        stateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound,
        EnemyAttack,
        EnemyAttackFinished,
    }
    #endregion

    #region Distance Checks
    public void SetAggroStatus(bool isAggreed)
    {
        IsAggreed = isAggreed;
    }

    public void SetSpecialAggroStatus(bool isSpecialAggro)
    {
        IsSpecialAggro = isSpecialAggro;
    }
    
    public void SetImpactStatus(bool isImpact)
    {
        IsImpact = isImpact;
    }
    #endregion

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicUpdate();
    }


}
