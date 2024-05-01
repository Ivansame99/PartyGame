using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lion Attack State", menuName = "Enemy Logic/Boss/Lion/Attack Logic/AttackState")]
public class LionAttack : EnemyAttackSOBase
{


    //COOLDOWN ATTACKS
    private int attackCount;
    private float attackTimer;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float attackForce;

    bool isAttacking;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);

    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.isStopped = true;
        attackCount = 0;
        attackTimer = timeBetweenAttacks;
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.rb.velocity = Vector3.zero;
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        //A BIT OF COOLDOWN WHEN ATTACK FINISHED
        if (!enemy.isDead)
        {
            if(attackCount >= 4)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);

        if (attackTimer <= 0)
        {   
            enemy.animator.SetInteger("AttackCombo", attackCount);
            isAttacking = true;
            attackTimer = timeBetweenAttacks;
        }
        else attackTimer -= Time.deltaTime;
    }
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();


        if (isAttacking)
        {
            
            attackCount++;
            enemy.rb.AddForce(enemy.transform.forward * attackForce, ForceMode.Impulse);
            enemy.rb.velocity = Vector3.zero;
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
