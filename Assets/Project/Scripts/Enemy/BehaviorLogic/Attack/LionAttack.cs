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
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                isAttacking = true;
                break;
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                enemy.stateMachine.ChangeState(enemy.idleState);
                break;
        }
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.isStopped = true;
        attackCount = 0;
        attackTimer = timeBetweenAttacks;
        enemy.animator.ResetTrigger("Chase");
        enemy.animator.SetTrigger("Attack");
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.rb.velocity = Vector3.zero;
        //enemy.attackCollider.enabled = false;
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
            enemy.rb.AddForce(enemy.transform.forward * attackForce, ForceMode.Impulse);
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
