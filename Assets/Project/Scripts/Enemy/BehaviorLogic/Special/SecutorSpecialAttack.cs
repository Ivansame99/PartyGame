using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpecialAttack", menuName = "Enemy Logic/Secutor/Attack Logic/Special Attack")]
public class SecutorSpecialAttack : EnemySpecialAttackSOBase
{
    [SerializeField] private ParticleSystem areaAttackParticles;

    private float attackTimer;
    [SerializeField] private float attackTime;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetBool("Special",true);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.animator.SetBool("Special", false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if(!enemy.isDead)
        {
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Special"))
            {
                enemy.stateMachine.ChangeState(enemy.stunnedState);
            }

            else if (attackTimer <= 0)
            {
                enemy.SetDamagedStatus(false);
                enemy.stateMachine.ChangeState(enemy.stunnedState);
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        } else enemy.stateMachine.ChangeState(enemy.deathState);
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
