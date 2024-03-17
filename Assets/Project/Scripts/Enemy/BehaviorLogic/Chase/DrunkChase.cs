using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Chase", menuName = "Enemy Logic/Drunk/Chase Logic/Chase To Player")]
public class DrunkChase : EnemyChaseSOBase
{
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.isStopped = false;
    }
    public override void DoFrameUpdateLogic()
    {
        if (enemy.playerPos != null) enemy.MoveEnemy(enemy.playerPos.position);

        if (!enemy.isDead)
        {
            if (enemy.playerPos != null) enemy.MoveEnemy(enemy.playerPos.position);
            CheckingStates();
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.deathState);
        }
    }
    void CheckingStates()
    {
        if (enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }

    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.isStopped = true;
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
