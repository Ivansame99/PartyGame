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
        Debug.Log(enemy.playerPos);
        //playerPos = enemy.target.player.position;

        enemy.MoveEnemy(enemy.playerPos.position);

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
