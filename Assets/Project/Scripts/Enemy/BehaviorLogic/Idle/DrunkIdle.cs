using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Idle", menuName = "Enemy Logic/Drunk/Status Logic/Idle")]
public class DrunkIdle : EnemyIdleSOBase
{
    //COOLDOWN ATTACKS
    private float attackTimer;
    [SerializeField] private float attackCooldown;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Idle");
        attackTimer = attackCooldown;
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            CheckingStates();
            Vector3 lookVector = enemy.playerPos.transform.position - transform.position;
            lookVector.y = transform.position.y;
            Quaternion rot = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);

            if (attackTimer <= 0)
            {
                if (enemy.IsSpecialAggro) enemy.stateMachine.ChangeState(enemy.specialAttackState);
                if (enemy.IsAggreed && !enemy.IsSpecialAggro) enemy.stateMachine.ChangeState(enemy.attackState);
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
    }
    void CheckingStates()
    {

        if (!enemy.IsAggreed && !enemy.IsSpecialAggro)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
        if (enemy.IsDamaged)
        {
            enemy.stateMachine.ChangeState(enemy.damageState);
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
}
