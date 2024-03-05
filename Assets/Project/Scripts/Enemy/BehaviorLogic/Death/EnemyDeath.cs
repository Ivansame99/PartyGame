using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDeath", menuName = "Enemy Logic/General/Enemy Death")]
public class EnemyDeath : EnemyDeathSOBase
{
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.Death:
                Death();
                break;
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Die");
        Debug.Log("entra");
    }

    void Death()
    {
        enemy.enemyTarget.DecreasePlayerTarget(enemy.enemyTarget.player.name);
        Destroy(gameObject);
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
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
