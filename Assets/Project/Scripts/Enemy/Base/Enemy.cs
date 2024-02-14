using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public NavMeshAgent agent { get; set; }
    public Rigidbody rb { get; set; }
    public bool state { get; set; }
    public Transform playerPos { get; set; }
    public Transform playerPos2 { get; set; }
    public EnemyTarget enemyTarget { get; set; }



    #region State Machine Variables
    public EnemyStateMachine stateMachine { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyAttackState attackState { get; set; }
    #endregion

    #region Chase Variables
    public float deg;
    public float speed;
    public float acceleration;
    public float angularSpeed;
    #endregion
    void Awake()
    {
        //Initialize State Machine
        stateMachine = new EnemyStateMachine();

        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }
    void Start()
    {
        //Get Components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        //Initialize Health
        CurrentHealth = MaxHealth;

        //Initialize State Machine
        stateMachine.Initialize(chaseState);
        Debug.Log("Enemy State Machine Initialized");
        
        
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

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState.PhysicUpdate();
    }

}
