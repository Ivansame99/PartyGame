using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaterAttackState : EnemyState
{
    public EnemyWaterAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyWaterAttackBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyWaterAttackBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.enemyWaterAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemy.enemyWaterAttackBaseInstance.DoPhysicsLogic();
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.enemyWaterAttackBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
