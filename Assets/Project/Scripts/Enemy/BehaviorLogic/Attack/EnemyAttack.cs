using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy Logic/Attack Logic/Enemy Attack")]
public class EnemyAttack : EnemyAttackSOBase
{
    [SerializeField] ParticleSystem areaAttackParticles;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);

        if(triggerType == Enemy.AnimationTriggerType.EnemyAttack)
        {
            Attack();
        }
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Attack");
        //enemy.AgentState(false);
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
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

    private void Attack()
    {
        Instantiate(areaAttackParticles, enemy.transform.position, Quaternion.identity);
    }
}

