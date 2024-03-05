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

    //COMBAT INTERFICE
    public PowerController powerController { get; set; }

    #region State Machine Variables
    //GENERAL COMUN STATES
    public EnemyStateMachine stateMachine { get; set; }
    public EnemyChaseState chaseState { get; set; }
    public EnemyAttackState attackState { get; set; }
    public EnemySpecialAttackState specialAttackState { get; set; }
    public EnemyStunnedState stunnedState { get; set; }
    #endregion

    #region SO Variables

    //GENERAL COMUN STATES
    public EnemyChaseSOBase enemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase enemyAttackBaseInstance { get; set; }
    public EnemySpecialAttackSOBase enemySpecialAttackBaseInstance { get; set; }
    public EnemyStunedSOBase enemyStunedBaseInstance { get; set; }
    

    #endregion


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
    
    #endregion

    #region Combat Functions
    public float GetPowerDamage()
    {
        return powerController.PowerDamage();
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
