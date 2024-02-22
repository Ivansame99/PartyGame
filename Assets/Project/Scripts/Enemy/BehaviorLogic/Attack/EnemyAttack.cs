using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy Logic/Attack Logic/Enemy Attack")]
public class EnemyAttack : EnemyAttackSOBase
{
    [SerializeField] ParticleSystem areaAttackParticles;
    [SerializeField] GameObject expansiveWave;
    [SerializeField] float expansiveScale;

    [SerializeField] private float enemyBaseDamage;
    [SerializeField] private float enemyBasePushForce;
    private Vector3 playerLastPoint;
    private Vector3 playerDir;
    private Vector3 lookDir;

    private float randomAttack;
    private bool isAttacking;

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
        var rotation = Quaternion.LookRotation(enemy.playerPos.position - transform.position);
        rotation.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.1f);

        //enemy.AgentState(false);
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

        if (attackTimer <= 0 && !isAttacking)
        {
            isAttacking = true;
            randomAttack = Random.Range(1,1);

            
            playerDir = (enemy.playerPos.position - transform.position).normalized;




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
            switch (randomAttack)
            {
                case 1:
                    enemy.animator.SetInteger("AttackType", 1);
                    break;
                case 2:
                    ChargeAttack();
                    break;

            }

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
        if (randomAttack == 1)
        {
            Instantiate(areaAttackParticles, enemy.transform.position, Quaternion.identity);
            GameObject waveAttack = Instantiate(expansiveWave, enemy.transform.position, Quaternion.identity);
            
            if(waveAttack != null)
            {
                waveAttack.transform.localScale += new Vector3(expansiveScale,0, expansiveScale);
                Destroy(waveAttack, 2f);
            }
            if (enemy.IsImpact)
            {
                Debug.Log("menos 10 de vida");
                Slash();
            }
        }

    }

    private void ChargeAttack()
    {
        enemy.rb.MovePosition(transform.position + playerDir * 20f * Time.fixedDeltaTime);


        //AttackFinished();
        
    }

    private void AttackFinished()
    {
        enemy.animator.SetInteger("AttackType", 0);
        attackTimer = attackCooldown;
        randomAttack = 0;
        isAttacking = false;
    }
    public void Slash()
    {
        enemy.slashController.finalDamage = enemyBaseDamage + enemy.powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
        enemy.slashController.pushForce = enemyBasePushForce;
    }
}

