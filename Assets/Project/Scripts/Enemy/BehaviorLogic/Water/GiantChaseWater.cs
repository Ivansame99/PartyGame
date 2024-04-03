using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Giant Water Chase", menuName = "Enemy Logic/Giant/Water Logic/Chase")]
public class GiantChaseWater : EnemyWaterChaseSOBase
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float angularSpeed;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.agent.speed / 3;
        // enemy.agent.acceleration = acceleration;
        //enemy.agent.angularSpeed = angularSpeed;

    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (!enemy.isDead)
        {
            if(!enemy.OnWater) enemy.stateMachine.ChangeState(enemy.chaseState);

            if (enemy.playerPos != null) enemy.MoveEnemy(enemy.playerPos.position);

            if (enemy.IsAggreed)
            {
                enemy.stateMachine.ChangeState(enemy.attackState);
            }

            if (enemy.IsSpecialAggro)
            {
                enemy.stateMachine.ChangeState(enemy.specialAttackState);
            }
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.deathState);
        }

        //Debug.Log("Sigo");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.isStopped = true;
        enemy.agent.speed = enemy.agent.speed * 3;
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
