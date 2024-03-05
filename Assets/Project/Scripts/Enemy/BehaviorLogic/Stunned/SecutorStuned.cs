using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySecutorStuned", menuName = "Enemy Logic/Secutor/Status Logic/Secutor Stuned")]
public class SecutorStuned : EnemyStunedSOBase
{

    //COOLDOWN STUNED
    private float stunedTimer;
    [SerializeField] private float stunTime;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetInteger("AnimationType", 4);
        stunedTimer = stunTime;
        Debug.Log("Stuned");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (stunedTimer <= 0)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }
        else
        {
            stunedTimer -= Time.deltaTime;
        }
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


