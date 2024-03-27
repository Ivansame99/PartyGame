using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Chase", menuName = "Enemy Logic/Drunk/Water Logic/Chase")]
public class DrunkChaseWater : EnemyWaterChaseSOBase
{
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Chase");
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.agent.speed / 2;
    }
    public override void DoFrameUpdateLogic()
    {
        if (!enemy.isDead)
        {
            CheckingStates();
            Debug.Log("Chase Water");
            if (enemy.playerPos != null) enemy.MoveEnemy(enemy.playerPos.position);
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.deathState);
        }
    }
    void CheckingStates()
    {
        if (!enemy.OnWater) enemy.stateMachine.ChangeState(enemy.chaseState);

        if (enemy.IsSpecialAggro) enemy.stateMachine.ChangeState(enemy.specialAttackState);

        if (enemy.IsAggreed && !enemy.IsSpecialAggro) enemy.stateMachine.ChangeState(enemy.attackState);

        if (enemy.IsDamaged) enemy.stateMachine.ChangeState(enemy.damageState);

    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.isStopped = true;
        enemy.agent.speed = enemy.agent.speed * 2;
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
