using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaterChaseState : EnemyState
{
    public EnemyWaterChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyWaterChaseBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyWaterChaseBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.enemyWaterChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemy.enemyWaterChaseBaseInstance.DoPhysicsLogic();
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.enemyWaterChaseBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
