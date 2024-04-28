using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lion Idle", menuName = "Enemy Logic/Boss/Lion/Status Logic/Idle")]
public class LionIdle : EnemyIdleSOBase
{
    private int behaviorIndex;
    private int lastBehaviorIndex;

    [SerializeField] private float idleDuration;
    private float idleTimer;

    private bool fixBehavior;
    private bool firstTime;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.players.Count);

        enemy.bossTarget = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;
        behaviorIndex = Random.Range(0, 3);
        idleTimer = idleDuration;
        Debug.Log("lion idle state");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        lastBehaviorIndex = behaviorIndex;
        fixBehavior = false;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        Debug.Log("Sigo aqui");
        Debug.Log(behaviorIndex);
        if(!enemy.isDead)
        {
            if (behaviorIndex == lastBehaviorIndex && !fixBehavior && firstTime)
            {
                behaviorIndex = Random.Range(0, 3);
                fixBehavior = true;
            }
            else if (!firstTime) fixBehavior = true;

            if (idleTimer <= 0)
            {
                switch(behaviorIndex)
                {
                    case 0:
                        enemy.stateMachine.ChangeState(enemy.bossTorusState);
                        break;
                    case 1:
                        enemy.stateMachine.ChangeState(enemy.specialAttackState);
                        break;
                    case 2:
                        enemy.stateMachine.ChangeState(enemy.bossDistanceAttackState);
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
