using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lion Chase", menuName = "Enemy Logic/Boss/Lion/Chase Logic/Chase To Player")]
public class LionChase : EnemyChaseSOBase
{
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float angularSpeed = 120f;

    Vector3 playerDir;
    private Transform player;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        enemy.agent.speed = speed;
        enemy.agent.acceleration = acceleration;
        enemy.agent.angularSpeed = angularSpeed;

        enemy.randomPlayerTarget = Random.Range(0, enemy.enemyDirector.players.Count);
        player = enemy.enemyDirector.players[enemy.randomPlayerTarget].transform;
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            if (player != null) enemy.MoveEnemy(player.position);
            
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
