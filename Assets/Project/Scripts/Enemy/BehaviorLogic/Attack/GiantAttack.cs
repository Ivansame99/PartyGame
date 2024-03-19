using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Giant Attack", menuName = "Enemy Logic/Giant/Attack Logic/Attack")]
public class GiantAttack : EnemyAttackSOBase
{
    [Header("Wave prefabs")]
    [SerializeField] ParticleSystem areaAttackParticles;
    [SerializeField] ParticleSystem FeedbackParticles;
    [SerializeField] ParticleSystem SmokeFeedbackParticles;
    [SerializeField] ParticleSystem groundHit;
    [SerializeField] GameObject expansiveWave;

    [Header("Wave Attack parameters")]
    [SerializeField] float waveSpeed;
    [SerializeField] float waveTimeLife;

    //Gameobject to instantiate
    private GameObject waveAttack;
    
    //Attack flag
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
        if(waveAttack != null) Destroy(waveAttack);
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
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attacking"))
            {
                AttackFinished();
            }

            if (attackTimer <= 0 && !isAttacking)
            {
                isAttacking = true;
                enemy.animator.SetInteger("AttackType", 1);
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }

            if (waveAttack != null)
            {
                waveAttack.transform.localScale += new Vector3(waveSpeed, waveSpeed, waveSpeed);
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
     //   Instantiate(areaAttackParticles, enemy.transform.position, Quaternion.identity);
       
        waveAttack = Instantiate(expansiveWave, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.2f, enemy.transform.position.z), Quaternion.identity);
        Instantiate(FeedbackParticles, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.identity);
        Instantiate(SmokeFeedbackParticles, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.identity);
        enemy.giantAudioManager.PlayStomp();
        Instantiate(groundHit, enemy.transform.position, groundHit.transform.rotation);
        Torus torus = waveAttack.GetComponent<Torus>();
        torus.finalDamage = torus.baseDamage + enemy.GetPowerDamageScale(); //cambiar escalado de poder
        torus.SetPushForce(torus.pushForce);
        torus.owner = enemy.gameObject;

        Destroy(waveAttack, waveTimeLife);
    }
    private void AttackFinished()
    {
        enemy.animator.SetInteger("AttackType", 0);
        attackTimer = attackCooldown;
        isAttacking = false;
    }
}

