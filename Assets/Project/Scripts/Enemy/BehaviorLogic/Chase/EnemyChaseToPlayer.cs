using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase To Player", menuName = "Enemy Logic/Chase Logic/Chase To Player")]
public class EnemyChaseToPlayer : EnemyChaseSOBase
{
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float angularSpeed;

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

    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (enemy.IsAggreed)
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }

        enemy.MoveEnemy(enemy.playerPos.position);
        //Debug.Log("Sigo");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.agent.isStopped = true;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
