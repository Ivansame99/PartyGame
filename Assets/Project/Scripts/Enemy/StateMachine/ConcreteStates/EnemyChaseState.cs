using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
 
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyChaseBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyChaseBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.enemyChaseBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemy.enemyChaseBaseInstance.DoPhysicsLogic();
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.enemyChaseBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

}
