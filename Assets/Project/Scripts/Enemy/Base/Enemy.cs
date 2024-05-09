using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckeable, ICombat, IBoss
{
	//HEALTH INTERFACE
	[field: Header("Health parameters")]
	[field: SerializeField] public float maxHealth { get; set; } = 100f;
	[field: SerializeField] public float inmuneTime { get; set; }
	public float currentHealth { get; set; }
	public bool isDead { get; set; }
	public bool IsDamaged { get; set; }
	public int randomPlayerTarget { get; set; }
	public Transform patrolPoint { get; set; }

    //COMPONENTS INTERFACE
    public NavMeshAgent agent { get; set; }
	public Rigidbody rb { get; set; }
	public EnemyHealth enemyHealthController { get; set; }
	public bool state { get; set; }
	public EnemyDirector enemyDirector { get; set; }
	public Transform playerPos { get; set; }
	public Transform playerPos2 { get; set; }
	public Transform bossTarget { get; set; }
	public EnemyTargetController enemyTargetController { get; set; }
	public Animator animator { get; set; }

	//AGGRO INTERFACE
	public bool IsAggreed { get; set; }
	public bool IsSpecialAggro { get; set; }
	public bool OnWater { get; set; }

	//COMBAT INTERFICE
	public PowerController powerController { get; set; }

	#region State Machine Variables
	//GENERAL COMUN STATES
	public EnemyStateMachine stateMachine { get; set; }
	public EnemyIdleState idleState { get; set; }
	public EnemyChaseState chaseState { get; set; }
	public EnemyPreAttackState preAttackState { get; set; }
	public EnemyAttackState attackState { get; set; }
	public EnemySpecialAttackState specialAttackState { get; set; }
	public EnemyStunnedState stunnedState { get; set; }
	public EnemyDamageState damageState { get; set; }
	public EnemyDeathState deathState { get; set; }
	public BossTorus bossTorusState { get; set; }
	public BossDistanceAttack bossDistanceAttackState { get; set; }

	//WATER STATES
	public EnemyWaterChaseState waterChaseState { get; set; }
	public EnemyWaterAttackState waterAttackState { get; set; }

	//BOSS STATES

	#endregion

	#region SO Variables

	//GENERAL COMUN STATES
	public EnemyChaseSOBase enemyChaseBaseInstance { get; set; }
	public EnemyIdleSOBase enemyIdleBaseInstance { get; set; }
	public EnemyPreAttackSOBase enemyPreAttackBaseInstance { get; set; }
	public EnemyAttackSOBase enemyAttackBaseInstance { get; set; }
	public EnemySpecialAttackSOBase enemySpecialAttackBaseInstance { get; set; }
	public EnemyStunedSOBase enemyStunedBaseInstance { get; set; }
	public EnemyDamageSOBase enemyDamageBaseInstance { get; set; }
	public EnemyDeathSOBase enemyDeathBaseInstance { get; set; }

	//BOSS STATES
	public BossTorusSOBase bossTorusBaseInstance { get; set; }
	public BossDistanceAttackSOBase bossDistanceAttackBaseInstance { get; set; }


	//WATER STATES
	public EnemyWaterChaseSOBase enemyWaterChaseBaseInstance { get; set; }
	public EnemyWaterAttackSOBase enemyWaterAttackBaseInstance { get; set; }

	//ENEMY UNIQUE COMPONENTS
	public SecutorAudioManager secutorAudioManager;
	public GiantAudioManager giantAudioManager;
	public DrunkAudioManager drunkAudioManager;

	public ParticleSystem trailSand;
    public BoxCollider attackCollider;
    #endregion


    #region Health/Damage
    public void SetDamagedStatus(bool isDamaged)
	{
		IsDamaged = isDamaged;
	}
	#endregion

	#region Movement Functions
	public void MoveEnemy(Vector3 position)
	{
		agent.SetDestination(position);
	}

	public Transform RandomPatrol()
	{
        int randomPoint = Random.Range(0, enemyDirector.enemyPatrolPoints.Length);
        patrolPoint = enemyDirector.enemyPatrolPoints[randomPoint];

		return patrolPoint;
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
		Death,
		EnemyAttack,
		EnemyAttackFinished,
	}
	#endregion

	#region Trigger Checks
	public void SetAggroStatus(bool isAggreed)
	{
		IsAggreed = isAggreed;
	}

	public void SetSpecialAggroStatus(bool isSpecialAggro)
	{
		IsSpecialAggro = isSpecialAggro;
	}
	public void SetWaterStatus(bool isWater)
	{
		OnWater = isWater;
	}
    #endregion

    #region Combat Functions
    public float GetPowerDamageScale()
	{
		return powerController.PowerDamage();
	}

	public float GetPoweLevel()
	{
		return powerController.GetCurrentPowerLevel();
	}
	public float GetPowerHealth()
	{
		return powerController.PowerHealth();
	}

    #endregion

    private void Update()
	{
		stateMachine.CurrentEnemyState.FrameUpdate();
	}

	private void FixedUpdate()
	{
		//Debug.Log(stateMachine.CurrentEnemyState);
		stateMachine.CurrentEnemyState.PhysicUpdate();
	}


}
