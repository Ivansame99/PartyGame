using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lion Chase", menuName = "Enemy Logic/Boss/Lion/Chase Logic/Chase To Player")]
public class LionChase : EnemyChaseSOBase
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float angularSpeed = 120f;

    [SerializeField] private float closeDistance = 4f;


    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.enabled = true;
        enemy.agent.speed = speed;
        enemy.agent.acceleration = acceleration;
        enemy.agent.angularSpeed = angularSpeed;

    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            if (enemy.bossTarget != null) enemy.MoveEnemy(enemy.bossTarget.position);
            if(Vector3.Distance(enemy.transform.position, enemy.bossTarget.position) <= closeDistance)
            {
                enemy.stateMachine.ChangeState(enemy.attackState);
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();

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
