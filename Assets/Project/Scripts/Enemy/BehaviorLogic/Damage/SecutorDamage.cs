using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Secutor Take Damage", menuName = "Enemy Logic/Secutor/Status Logic/Damage")]
public class SecutorDamage : EnemyDamageSOBase
{
    private float stunedTimer;
    [SerializeField] private float stunTime;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public void ChangeState()
    {
        enemy.stateMachine.ChangeState(enemy.chaseState);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Damaged");
        stunedTimer = stunTime;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.rb.velocity = Vector3.zero;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (stunedTimer <= 0)
        {
            enemy.SetDamagedStatus(false);
            CheckingStates();    
        }
        else
        {
            stunedTimer -= Time.deltaTime;
        }
    }
    void CheckingStates()
    {

        if (enemy.IsSpecialAggro)
        {
            enemy.stateMachine.ChangeState(enemy.specialAttackState);
        }

        else if (enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        else { enemy.stateMachine.ChangeState(enemy.chaseState); }
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
