using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Secutor Attack", menuName = "Enemy Logic/Secutor/Attack Logic/Attack")]
public class SecutorAttack : EnemyAttackSOBase
{
    [SerializeField] private float attackSpeed;
    [SerializeField] GameObject feedbackAttack;
    private GameObject feedback;

    private bool isAttacking,isFinished;

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
        isFinished = false;
        enemy.animator.SetInteger("AnimationType", 1);
        feedback = Instantiate(feedbackAttack,enemy.transform);
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        //A BIT OF COOLDOWN WHEN ATTACK FINISHED
        if(!enemy.isDead)
        {
            Debug.Log("1");
            if(enemy.IsDamaged)
            {
                if(feedback != null) Destroy(feedback);
                isFinished = false;
                enemy.stateMachine.ChangeState(enemy.damageState);
                
            }
            if (isFinished)
            {
                if (attackTimer <= 0)
                {
                    isFinished = false;
                    enemy.rb.velocity = Vector3.zero;
                    Debug.Log("2");
                    enemy.stateMachine.ChangeState(enemy.chaseState);
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }
            }
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.deathState);
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
        attackTimer = attackCooldown;
        isFinished = true;
    }
}
