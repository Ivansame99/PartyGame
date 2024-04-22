using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTorus : EnemyState
{
    public BossTorus(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.bossTorusBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.bossTorusBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.bossTorusBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        enemy.bossTorusBaseInstance.DoPhysicsLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.bossTorusBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }
}
