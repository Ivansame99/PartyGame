using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunnedState : EnemyState
{
    public EnemyStunnedState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyStunedBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyStunedBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.enemyStunedBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemy.enemyStunedBaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.enemyStunedBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
