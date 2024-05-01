using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDistanceAttack : EnemyState
{
    public BossDistanceAttack(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.bossDistanceAttackBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.bossDistanceAttackBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.bossDistanceAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemy.bossDistanceAttackBaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.bossDistanceAttackBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
