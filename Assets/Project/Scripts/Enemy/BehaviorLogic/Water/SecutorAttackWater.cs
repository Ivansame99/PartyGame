using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Secutor Attack", menuName = "Enemy Logic/Secutor/Water Logic/Attack")]
public class SecutorAttackWater : EnemyWaterAttackSOBase
{
    [SerializeField] private float attackSpeed;


    private bool isAttacking, isFinished;

    //COOLDOWN ATTACKS
    private float attackTimer;
    [SerializeField] private float attackCooldown;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Attack");
        enemy.secutorAudioManager.PlaySwordWhoosh();
        isAttacking = true;
        attackTimer = attackCooldown;
        Debug.Log("isWater");
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();

    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        //A BIT OF COOLDOWN WHEN ATTACK FINISHED
        if (!enemy.isDead)
        {
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                enemy.rb.velocity = Vector3.zero;
            }

            if (enemy.IsDamaged)
            {
                enemy.stateMachine.ChangeState(enemy.damageState);
            }

            if (!isAttacking)
            {
                if (attackTimer <= 0)
                {
                    if (!enemy.OnWater) enemy.stateMachine.ChangeState(enemy.chaseState);
                    else enemy.stateMachine.ChangeState(enemy.waterChaseState);


                }
                else
                {
                    attackTimer -= Time.deltaTime;

                }
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);


    }
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();

        if (isAttacking)
        {
            enemy.rb.AddForce(enemy.transform.forward * attackSpeed, ForceMode.Impulse);
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
