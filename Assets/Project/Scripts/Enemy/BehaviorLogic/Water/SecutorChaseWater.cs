using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Secutor Chase Water", menuName = "Enemy Logic/Secutor/Water Logic/Chase")]
public class SecutorChaseWater : EnemyWaterChaseSOBase
{
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float angularSpeed = 120f;

    Vector3 playerDir;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //Set chase animation
        enemy.agent.isStopped = false;

        enemy.animator.SetTrigger("Chase");
        CheckingStates();

        //Set animation
        //enemy.animator.SetInteger("AnimationType",0);

        //Agent configuration
        enemy.agent.speed = enemy.agent.speed / 2;
        enemy.agent.acceleration = acceleration;
        enemy.agent.angularSpeed = angularSpeed;
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            if (enemy.playerPos != null) enemy.MoveEnemy(enemy.playerPos.position);
            CheckingStates();
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.deathState);
        }
    }
    void CheckingStates()
    {
        if(!enemy.OnWater) enemy.stateMachine.ChangeState(enemy.chaseState);

        if (enemy.IsDamaged)
        {
            enemy.stateMachine.ChangeState(enemy.damageState);
        }

        if (enemy.IsSpecialAggro)
        {
            enemy.stateMachine.ChangeState(enemy.specialAttackState);
        }

        if (enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.preAttackState);
        }
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.isStopped = true;
        enemy.agent.speed = enemy.agent.speed * 2;
        //enemy.animator.SetTrigger("Idle");
        //enemy.MoveEnemy(transform.position);

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
