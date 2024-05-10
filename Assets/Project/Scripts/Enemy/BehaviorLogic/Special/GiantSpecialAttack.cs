using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "Giant Special", menuName = "Enemy Logic/Giant/Attack Logic/Special Attack")]
public class GiantSpecialAttack : EnemySpecialAttackSOBase
{
    [SerializeField] private float chargeSpeed = 35f;
    [SerializeField] private float impulseForce = 50f;
    [SerializeField] private float colisionDistance;
    [SerializeField] private float atkDuration;

    [SerializeField] private float preChargeDuration;
    private float preChargeTimer;
    private float atkTimer;

    private bool isAttacking;
    private bool isStunned;
    private Vector3 playerDir;

    private RaycastHit hit;

    [Header("Feedback prefab")]
    [SerializeField] private GameObject feedbackAttack;
    private GameObject feedback;

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
        feedback = Instantiate(feedbackAttack, enemy.transform);
        atkTimer = atkDuration;
        preChargeTimer = preChargeDuration;
        isAttacking = false;
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
        
        if (!isAttacking || (!isAttacking && preChargeTimer > 0))
        {
            transform.LookAt(new Vector3(enemy.playerPos.position.x, 0, enemy.playerPos.position.z));
            playerDir = (enemy.playerPos.position - transform.position).normalized;
            playerDir.y = 0;
        }else preChargeTimer -= Time.deltaTime;
        if(isAttacking)
        {

            if (atkTimer <= 0)
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
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
                enemy.stateMachine.ChangeState(enemy.chaseState);
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
        if (feedback != null) Destroy(feedback);
        enemy.animator.SetInteger("AttackType", 3);
    }

}
