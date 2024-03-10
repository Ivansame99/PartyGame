using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Attack", menuName = "Enemy Logic/Drunk/Attack Logic/Attack")]
public class DrunkAttack : EnemyAttackSOBase
{
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {

        }
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("Drunk Attack Enter");
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            if (!enemy.IsAggreed)
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.deathState);
        }
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
