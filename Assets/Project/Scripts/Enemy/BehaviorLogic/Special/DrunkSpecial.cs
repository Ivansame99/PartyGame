using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EnemySpecialAttack", menuName = "Enemy Logic/Drunk/Attack Logic/Special Attack")]
public class DrunkSpecial : EnemySpecialAttackSOBase
{
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (enemy.isDead)
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
