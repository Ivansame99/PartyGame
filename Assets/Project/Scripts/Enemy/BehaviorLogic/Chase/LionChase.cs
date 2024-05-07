using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Lion Chase", menuName = "Enemy Logic/Boss/Lion/Chase Logic/Chase To Player")]
public class LionChase : EnemyChaseSOBase
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float angularSpeed = 120f;

    [SerializeField] private float closeDistance = 4f;
    [SerializeField] private float deg;
    private Vector3 direction;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.isStopped = false;
        enemy.agent.speed = speed;
        enemy.agent.acceleration = acceleration;
        enemy.agent.angularSpeed = angularSpeed;

        enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.players.Count);

        enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;
        enemy.animator.SetTrigger("Chase");
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            if (enemy.bossTarget == null)
            {
                enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.currentPlayers);
                enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;
            }
            if (enemy.bossTarget != null)
            {
                direction = enemy.bossTarget.position - transform.position;
                enemy.MoveEnemy(enemy.bossTarget.position);
            }

            if(Vector3.Distance(enemy.transform.position, enemy.bossTarget.position) <= closeDistance && EnemyVision(65f))
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
    private bool EnemyVision(float vision)
    {
        if (Mathf.Abs(Vector3.Angle(transform.forward, direction)) < vision)
        {
            return true;
        }
        else { return false; }

    }
}
