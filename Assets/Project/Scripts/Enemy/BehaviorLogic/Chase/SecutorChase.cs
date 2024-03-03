using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "Secutor Chase", menuName = "Enemy Logic/Secutor/Chase Logic/Chase To Player")]
public class SecutorChase : EnemyChaseSOBase
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float angularSpeed;

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
        enemy.animator.SetTrigger("Run");
        //enemy.animator.SetInteger("AnimationType", 0);

        //Agent configuration
        //enemy.agent.isStopped = false;
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }

        if (enemy.IsSpecialAggro)
        {
            //enemy.stateMachine.ChangeState(enemy.specialAttackState);
        }
        //if (enemy.playerPos != null) enemy.MoveEnemy(enemy.playerPos.position);

        //enemy.transform.LookAt(enemy.playerPos);
        if (enemy.playerPos != null) enemy.MoveEnemy(enemy.playerPos.position);

    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.isStopped = true;
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
