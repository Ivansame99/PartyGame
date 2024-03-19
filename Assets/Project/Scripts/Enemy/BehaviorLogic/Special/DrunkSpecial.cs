using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EnemySpecialAttack", menuName = "Enemy Logic/Drunk/Attack Logic/Special Attack")]
public class DrunkSpecial : EnemySpecialAttackSOBase
{
    private Vector3 lookVector;

    private float attackTimer;
    [SerializeField] private float attackCooldown;

    private bool finish;

    [SerializeField] GameObject feedbackAttack;
    private GameObject feedback;

    [SerializeField] GameObject pukeParticles;
    private GameObject puke;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetBool("Puke",true);


        attackTimer = attackCooldown;
        feedback = Instantiate(feedbackAttack, enemy.transform);
        finish = false;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        enemy.animator.SetBool("Puke", false);
        if(puke != null) Destroy(puke,2f);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {

            if(finish)
            {
                if (attackTimer <= 0)
                {
                    enemy.stateMachine.ChangeState(enemy.idleState);
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }
            }

            if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Puke") && !finish)
            {
                if (feedback != null) Destroy(feedback);
                puke = Instantiate(pukeParticles, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1, enemy.transform.position.z), enemy.transform.rotation);
                finish = true;
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
    }
    Vector3 WhoToLook()
    {
        Vector3 look;
        if (enemy.playerPos2 != null)
        {
            float distancePlayer1 = Vector3.Distance(enemy.playerPos.position, transform.position);
            float distancePlayer2 = Vector3.Distance(enemy.playerPos2.position, transform.position);
            if (distancePlayer1 < distancePlayer2) look = enemy.playerPos.transform.position - transform.position;
            else look = enemy.playerPos2.transform.position - transform.position;
        }else look = enemy.playerPos.transform.position;
        return look;

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
}
