using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Secutor Pre-Attack", menuName = "Enemy Logic/Secutor/Attack Logic/Pre-Attack")]
public class SecutorPreAttack : EnemyPreAttackSOBase
{
    [SerializeField] GameObject feedbackAttack;
    private GameObject feedback;

    private float preAttackTimer;
    [SerializeField] private float preAttackTime;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Feedback");
        feedback = Instantiate(feedbackAttack, enemy.transform);
        preAttackTimer = preAttackTime;
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        if (feedback != null) Destroy(feedback);
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead){
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Feedback"))
            {
                enemy.stateMachine.ChangeState(enemy.attackState);
            }

            else if (preAttackTimer <= 0)
            {
                if(!enemy.OnWater) enemy.stateMachine.ChangeState(enemy.attackState);
                else enemy.stateMachine.ChangeState(enemy.waterAttackState);
            }
            else
            {
                preAttackTimer -= Time.deltaTime;
            }

            if (enemy.IsDamaged)
            {
                enemy.stateMachine.ChangeState(enemy.damageState);
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
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
