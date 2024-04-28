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

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);

    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //enemy.agent.enabled = false;
        attackCount = 0;
        attackTimer = 0;
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
            if(attackCount >= 3)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
            if (attackTimer <= 0)
            {
                enemy.animator.SetInteger("AttackCombo",attackCount);
                enemy.rb.AddForce(enemy.transform.forward * attackForce, ForceMode.Impulse);

                attackCount++;
                attackTimer = timeBetweenAttacks;
            }
            else attackTimer -= Time.deltaTime;
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
