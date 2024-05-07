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
        enemy.animator.SetTrigger("Chase");
        enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.currentPlayers);
        enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;

        //behaviorIndex = Random.Range(0, 3);
        idleTimer = idleDuration;

        enemy.agent.isStopped = false;
        enemy.agent.speed = speed;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
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
            //behaviorIndex = 2;
            if (idleTimer <= 0)
            {
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
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);



    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
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
