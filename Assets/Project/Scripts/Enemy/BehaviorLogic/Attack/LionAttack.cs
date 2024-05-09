using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lion Attack State", menuName = "Enemy Logic/Boss/Lion/Attack Logic/AttackState")]
public class LionAttack : EnemyAttackSOBase
{
    [SerializeField] private float attackForceValor;
    [SerializeField, Range(0f, 1f)] private float comboMultiplyForce;

    //Combo controller
    private int attackCount;
    private float attackForce;
    private float sumForces;
    bool isAttacking;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                isAttacking = true;
                break;
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                enemy.stateMachine.ChangeState(enemy.idleState);
                enemy.rb.velocity = Vector3.zero;
                break;
        }
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //Reset parameters
        enemy.agent.isStopped = true;
        attackCount = 0;
        sumForces = 0;
        attackForce = attackForceValor;

        //Animator
        //enemy.animator.ResetTrigger("Chase");
        enemy.animator.SetTrigger("Attack");
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.rb.velocity = Vector3.zero;
        enemy.attackCollider.enabled = false;
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

    }
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();


        if (isAttacking)
        {
            if (attackCount > 1)
            {
                sumForces = attackForce * comboMultiplyForce;
                attackForce += sumForces;
            }
            enemy.rb.AddForce(transform.forward * attackForce, ForceMode.Impulse);
            enemy.attackCollider.enabled = true;
            attackCount++;
            sumForces = 0;
            isAttacking = false;
            
        }
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
