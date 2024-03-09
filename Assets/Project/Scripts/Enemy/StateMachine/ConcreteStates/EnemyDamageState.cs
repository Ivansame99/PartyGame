using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageState : EnemyState
{
    public EnemyDamageState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyDamageBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyDamageBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.enemyDamageBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemy.enemyDamageBaseInstance.DoPhysicsLogic();
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.enemyDamageBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
