using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "EnemySpecialAttack", menuName = "Enemy Logic/Giant/Attack Logic/Enemy Special Attack")]
public class EnemySpecialAttack : EnemySpecialAttackSOBase
{
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float impulseForce;
    private float colisionDistance = 0.1f;

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
   
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.IsSpecialAggro = false;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        
        if (!isAttacking)
        {
            transform.LookAt(new Vector3(enemy.playerPos.position.x, 0, enemy.playerPos.position.z));
            playerDir = (enemy.playerPos.position - transform.position).normalized;
            playerDir.y = 0;
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

        if (isAttacking && Physics.Raycast(transform.position, transform.forward, out hit, 1.5f))
        {
            // Verificar si el objeto golpeado tiene el tag "Wall"
            if (hit.collider.CompareTag("Wall"))
            {
                isAttacking = false;
                // Apply force impulse
                Vector3 backwardForce = -transform.forward * impulseForce;
                enemy.rb.AddForce(backwardForce, ForceMode.Impulse);
                //enemy.stateMachine.ChangeState(enemy.chaseState);
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
        enemy.animator.SetInteger("AttackType", 0);
    }
}
