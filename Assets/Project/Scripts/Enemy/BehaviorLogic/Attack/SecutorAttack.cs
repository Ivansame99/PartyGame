using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Secutor Attack", menuName = "Enemy Logic/Secutor/Attack Logic/Secutor Attack")]
public class SecutorAttack : EnemyAttackSOBase
{
    [SerializeField] private float attackSpeed;
    [SerializeField] GameObject feedbackAttack;
    private GameObject feedback;

    private bool isAttacking,isStarted;

    //COOLDOWN ATTACKS
    private float attackTimer;
    [SerializeField] private float attackCooldown;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                Attack();
                break;
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                AttackFinished();
                break;
        }
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        feedback = Instantiate(feedbackAttack,enemy.transform);
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.IsAggreed && !isAttacking)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }

        if (attackTimer <= 0 && !isStarted)
        {
            enemy.animator.SetTrigger("Attack");
            isStarted = true;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }


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

    private void Attack()
    {
        Destroy(feedback);
        isAttacking = true;
        
    }
    private void AttackFinished()
    {
        isStarted = false;
        attackTimer = attackCooldown;
        Debug.Log("acabo");
    }
}
