using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Lion Idle", menuName = "Enemy Logic/Boss/Lion/Status Logic/Idle")]
public class LionIdle : EnemyIdleSOBase
{
    private int behaviorIndex;
    private int lastBehaviorIndex;

    [SerializeField] private float idleDuration;
    private float idleTimer;

    private bool fixBehavior;
    private bool firstTime;
    public float range;

    [SerializeField] private float speed;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.agent.isStopped = false;
        //enemy.animator.ResetTrigger("Idle");
        enemy.animator.SetTrigger("Idle");
        //enemy.animator.ResetTrigger("Idle");
        //enemy.animator.SetTrigger("Chase");
        enemy.MoveEnemy(enemy.RandomPatrol().transform.position);
        enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.currentPlayers);
        enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;
        int ola = Random.Range(0, enemy.enemyDirector.enemyPatrolPoints.Length);

        //behaviorIndex = Random.Range(0, 3);
        idleTimer = idleDuration;

        
        //enemy.agent.speed = speed;

        behaviorIndex = Random.Range(0, 4);
        //while (behaviorIndex != lastBehaviorIndex) behaviorIndex = Random.Range(0, 4);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        lastBehaviorIndex = behaviorIndex;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if(!enemy.isDead)
        {
            /*
            if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(transform.position, range, out point)) //pass in our centre point and radius of area
                {
                    enemy.agent.SetDestination(point);
                }
            }
            */

            //while(behaviorIndex != lastBehaviorIndex) behaviorIndex = Random.Range(0, 4);
            if (enemy.bossTarget == null)
            {
                enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.currentPlayers);
                enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;
            }

            behaviorIndex = Random.Range(0, 4);
            //behaviorIndex = 0;
            if (idleTimer <= 0)
            {
                enemy.agent.isStopped = true;
                switch(behaviorIndex)
                {
                    case 0:
                        enemy.stateMachine.ChangeState(enemy.bossTorusState);
                        break;
                    case 1:
                        enemy.stateMachine.ChangeState(enemy.bossDistanceAttackState);
                        break;
                    case 2:
                        enemy.stateMachine.ChangeState(enemy.chaseState);
                        break;
                    case 3:
                        enemy.stateMachine.ChangeState(enemy.specialAttackState);
                        break;
                }
            }
            else
            {
                idleTimer -= Time.deltaTime;
                enemy.rb.velocity = Vector3.zero;
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);



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
