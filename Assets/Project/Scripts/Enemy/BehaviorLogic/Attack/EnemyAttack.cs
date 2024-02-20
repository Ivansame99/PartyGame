using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy Logic/Attack Logic/Enemy Attack")]
public class EnemyAttack : EnemyAttackSOBase
{
    [SerializeField] ParticleSystem areaAttackParticles;

    [SerializeField] private float enemyBaseDamage;
    [SerializeField] private float enemyBasePushForce;
    

    private float randomAttack;
    private bool isAttacking;

    //COOLDOWN ATTACKS
    private float attackTimer;
    [SerializeField] private float attackCooldown;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                Attack();
                break;
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                AttackFinished();
                break;
        }
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        attackTimer = 0;
        
        //enemy.AgentState(false);
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (!enemy.IsAggreed && !isAttacking)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }

        if (attackTimer <= 0 && !isAttacking)
        {
            isAttacking = true;
            randomAttack = Random.Range(1,1);
            
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

        if (isAttacking)
        {
            switch (randomAttack)
            {
                case 1:
                    enemy.animator.SetInteger("AttackType", 1);
                    break;
            }

        }

    }
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }
    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
    }
    public override void ResetValues()
    {
        base.ResetValues();
    }

    private void Attack()
    {
        Instantiate(areaAttackParticles, enemy.transform.position, Quaternion.identity);
        if(enemy.IsImpact)
        {
            Debug.Log("menos 10 de vida");
            Slash();
        }
    }

    private void AttackFinished()
    {
        enemy.animator.SetInteger("AttackType", 0);
        attackTimer = attackCooldown;
        randomAttack = 0;
        isAttacking = false;
    }
    public void Slash()
    {
        enemy.slashController.finalDamage = enemyBaseDamage + enemy.powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
        enemy.slashController.pushForce = enemyBasePushForce;
    }
}

