using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable,ITriggerCheckeable
{
    //HEALTH INTERFACE
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }

    //MOVEMENT INTERFACE
    public NavMeshAgent agent { get; set; }
    public Rigidbody rb { get; set; }
    public bool state { get; set; }
    public Transform playerPos { get; set; }
    public Transform playerPos2 { get; set; }
    public EnemyTarget enemyTarget { get; set; }

    //AGGRO INTERFACE
    public bool IsAggreed { get; set; }
    public bool IsWithStrikingDistance { get; set; }

    #region State Machine Variables
    public EnemyStateMachine stateMachine { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyAttackState attackState { get; set; }
    #endregion

    #region SO Variables
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;

    public EnemyChaseSOBase enemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase enemyAttackBaseInstance { get; set; }
    #endregion

    #region Chase Variables

    #endregion
    void Awake()
    {
        //Initialize SO
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        //enemyAttackBaseInstance = Instantiate(enemyAttackBase);
        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }
    void Start()
    {
        //Initialize Health
        CurrentHealth = MaxHealth;

        //Get Components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        //Initialize SO
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);

        //Initialize State Machine
        stateMachine.Initialize(chaseState);        
    }
    #region Health/Damage
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
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
    }
    #endregion

    #region Distance Checks
    public void SetAggroStatus(bool isAggreed)
    {
        IsAggreed = isAggreed;
    }

    public void SetStrikingDistanceBool(bool isWithStrikingDistance)
    {
        IsWithStrikingDistance = isWithStrikingDistance;
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
