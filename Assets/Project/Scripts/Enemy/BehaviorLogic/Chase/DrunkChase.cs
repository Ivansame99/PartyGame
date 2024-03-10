using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Chase", menuName = "Enemy Logic/Drunk/Chase Logic/Chase To Player")]
public class DrunkChase : EnemyChaseSOBase
{
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        Debug.Log("Drunk Chase Enter");
    }
    public override void DoFrameUpdateLogic()
    {

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
