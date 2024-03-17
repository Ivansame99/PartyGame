using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Idle", menuName = "Enemy Logic/Drunk/Idle")]
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
        
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        attackTimer = attackCooldown;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            CheckingStates();
            enemy.transform.LookAt(new Vector3(enemy.playerPos.position.x, enemy.transform.position.y, enemy.playerPos.position.z));

            if (attackTimer <= 0)
            {
                enemy.stateMachine.ChangeState(enemy.attackState);
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
        if (!enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
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
