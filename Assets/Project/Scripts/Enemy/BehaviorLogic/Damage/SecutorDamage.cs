using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Secutor Take Damage", menuName = "Enemy Logic/Secutor/Status Logic/Damage")]
public class SecutorDamage : EnemyDamageSOBase
{
    [SerializeField] private float pushForce;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                ChangeState();
                break;
        }
    }
    public void ChangeState()
    {
        enemy.SetDamagedStatus(false);
        enemy.animator.SetBool("Damage", false);

        if (enemy.IsSpecialAggro)
        {
            enemy.stateMachine.ChangeState(enemy.specialAttackState);
        }

        else if (enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetBool("Damage",true);
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
