using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lion Special", menuName = "Enemy Logic/Boss/Lion/Attack Logic/Special Attack")]
public class LionSpecial : EnemySpecialAttackSOBase
{
    [SerializeField] private float chargeSpeed = 35f;
    [SerializeField] private float impulseForce = 50f;
    [SerializeField] private float colisionDistance;
    [SerializeField] private float atkDuration;
    private float atkTimer;

    private bool isAttacking;
    private bool isStunned;
    private Vector3 playerDir;

    private RaycastHit hit;

    [SerializeField] float preChargeTime;
    private float preChargeTimer;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                enemy.stateMachine.ChangeState(enemy.idleState);
                break;
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.isStopped = true;

        
        enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.players.Count);
        enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;

        atkTimer = atkDuration;
        preChargeTimer = preChargeTime;

        enemy.animator.SetTrigger("Charge");
        Debug.Log("Aqui");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.isStopped = false;
        isAttacking = false;
        enemy.rb.velocity = Vector3.zero;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        TimersFunction();
        if (enemy.bossTarget == null)
        {
            enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.currentPlayers);
            enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, colisionDistance))
        {
            // Verificar si el objeto golpeado tiene el tag "Wall"
            if (hit.collider.CompareTag("Wall"))
            {
                isAttacking = false;
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
    public void TimersFunction()
    {
        if (!isAttacking && preChargeTimer <= 0)
        {
            isAttacking = true;
            enemy.animator.SetTrigger("ChargeAtk");
        }
        else if (!isAttacking && preChargeTimer > 0)
        {
            transform.LookAt(new Vector3(enemy.bossTarget.position.x, 0, enemy.bossTarget.position.z));
            playerDir = (enemy.bossTarget.position - transform.position).normalized;
            playerDir.y = 0;
            preChargeTimer -= Time.deltaTime;
        }

        if (isAttacking && atkTimer <= 0)
        {
            isAttacking = false;
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
        else if (isAttacking && atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
    }
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        if(isAttacking)
        {
            Debug.Log("isacaharging");
            Vector3 currentVelocity = new Vector3(enemy.rb.velocity.x, 0f, enemy.rb.velocity.z);

            if (currentVelocity.magnitude > chargeSpeed)
            {
                currentVelocity = currentVelocity.normalized * chargeSpeed;
            }
            Vector3 targetVelocity = playerDir * chargeSpeed;
            Vector3 force = (targetVelocity - currentVelocity) / Time.fixedDeltaTime;
            enemy.rb.AddForce(force, ForceMode.Acceleration);
        }
        else enemy.rb.velocity = Vector3.zero;
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
