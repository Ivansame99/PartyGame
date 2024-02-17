using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Vector3 _playerPos;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
 
    }

    public override void EnterState()
    {
        base.EnterState();

        //enemy.MoveEnemy(enemy.playerPos.position);

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        enemy.MoveEnemy(enemy.playerPos.position);

        //enemy.stateMachine.ChangeState(enemy.attackState);
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

}
