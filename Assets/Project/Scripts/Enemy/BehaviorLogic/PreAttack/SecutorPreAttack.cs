using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Secutor Pre-Attack", menuName = "Enemy Logic/Secutor/Attack Logic/Pre-Attack")]
public class SecutorPreAttack : EnemyPreAttackSOBase
{
    [SerializeField] GameObject feedbackAttack;
    private GameObject feedback;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Feedback");
        feedback = Instantiate(feedbackAttack, enemy.transform);
        Debug.Log("Hello PreAttack");
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
