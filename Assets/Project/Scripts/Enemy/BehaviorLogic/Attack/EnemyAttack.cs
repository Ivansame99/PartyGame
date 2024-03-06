using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "Enemy Logic/Giant/Attack Logic/Enemy Attack")]
public class EnemyAttack : EnemyAttackSOBase
{
    [Header("Wave prefabs")]
    [SerializeField] ParticleSystem areaAttackParticles;
    [SerializeField] GameObject expansiveWave;


    //Gameobject to instantiate
    private GameObject waveAttack;
    
    //Attack flag
    private bool isAttacking;

    //COOLDOWN ATTACKS
    private float attackTimer;
    [SerializeField] private float attackCooldown;
    private bool animMirror;

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
        if (animMirror) animMirror = false;
        else animMirror = true;
        
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();


        if (!enemy.isDead)
        {
            if (!enemy.IsAggreed && !isAttacking)
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }

            if (attackTimer <= 0 && !isAttacking)
            {
                isAttacking = true;
                float randomValor = Random.Range(0, 2);
                if (randomValor == 0) enemy.animator.SetInteger("AttackType", 1);
                if(randomValor == 1) enemy.animator.SetInteger("AttackType", 4);
            }
            else
            {
                attackTimer -= Time.deltaTime;
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
        Instantiate(expansiveWave, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.2f, enemy.transform.position.z), Quaternion.identity);

        Torus torus = waveAttack.GetComponent<Torus>();
        torus.finalDamage = torus.baseDamage + enemy.GetPowerDamage(); //cambiar escalado de poder
        torus.SetPushForce(torus.pushForce);
        torus.owner = enemy.gameObject;

    }
    private void AttackFinished()
    {
        enemy.animator.SetInteger("AttackType", 0);
        attackTimer = attackCooldown;
        isAttacking = false;
    }
}

