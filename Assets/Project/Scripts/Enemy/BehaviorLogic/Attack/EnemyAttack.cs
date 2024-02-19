using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy Logic/Attack Logic/Enemy Attack")]
public class EnemyAttack : EnemyAttackSOBase
{
    [SerializeField] ParticleSystem areaAttackParticles;

    [SerializeField] private float enemyBaseDamage;
    [SerializeField] private float enemyBasePushForce;

    [SerializeField] float normalAttackCooldown;

    private float timerAttack;
    private float randomAttack;

    private bool isAttacking,attackStarted;

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
        if(enemy.IsImpact)
        {
            Debug.Log("menos 10 de vida");
            Slash();
        }
    }

    private void AttackFinished()
    {

    }
    public void Slash()
    {
        enemy.slashController.finalDamage = enemyBaseDamage + enemy.powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
        enemy.slashController.pushForce = enemyBasePushForce;
    }
}

