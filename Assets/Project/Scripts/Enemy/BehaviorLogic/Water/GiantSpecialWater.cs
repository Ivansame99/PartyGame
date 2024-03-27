using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Giant Water Special", menuName = "Enemy Logic/Giant/Water Logic/Special Attack")]
public class GiantSpecialWater : EnemyWaterAttackSOBase
{
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float impulseForce = 50f;
    [SerializeField] private float colisionDistance;
    [SerializeField] private float atkDuration;
    private float atkTimer;

    private bool isAttacking;
    private bool isStunned;
    private Vector3 playerDir;

    private RaycastHit hit;

    [SerializeField] float timeToStun;
    private float timer;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                ChargeAttack();
                break;
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetInteger("AttackType", 2);
        atkTimer = atkDuration;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.IsSpecialAggro = false;
        enemy.animator.SetInteger("AttackType", 0);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (!isAttacking)
        {
            transform.LookAt(new Vector3(enemy.playerPos.position.x, 0, enemy.playerPos.position.z));
            playerDir = (enemy.playerPos.position - transform.position).normalized;
            playerDir.y = 0;

            if (atkTimer <= 0)
            {
                if(!enemy.OnWater) enemy.stateMachine.ChangeState(enemy.chaseState);
                else enemy.stateMachine.ChangeState(enemy.waterChaseState);

            }
            else
            {
                atkTimer -= Time.deltaTime;
            }
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        if (isAttacking)
        {
            Vector3 currentVelocity = new Vector3(enemy.rb.velocity.x, 0f, enemy.rb.velocity.z);

            if (currentVelocity.magnitude > chargeSpeed)
            {
                currentVelocity = currentVelocity.normalized * chargeSpeed;
            }
            Vector3 targetVelocity = playerDir * chargeSpeed;
            Vector3 force = (targetVelocity - currentVelocity) / Time.fixedDeltaTime;
            enemy.rb.AddForce(force, ForceMode.Acceleration);
        }

        if (isAttacking && Physics.Raycast(transform.position, transform.forward, out hit, colisionDistance))
        {
            // Verificar si el objeto golpeado tiene el tag "Wall"
            if (hit.collider.CompareTag("Wall"))
            {
                isAttacking = false;
                if (!enemy.OnWater) enemy.stateMachine.ChangeState(enemy.chaseState);
                else enemy.stateMachine.ChangeState(enemy.waterChaseState);
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

    private void ChargeAttack()
    {
        isAttacking = true;
        enemy.animator.SetInteger("AttackType", 3);
    }
}