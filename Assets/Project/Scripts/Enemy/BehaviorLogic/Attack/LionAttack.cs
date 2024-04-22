using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lion Attack State", menuName = "Enemy Logic/Boss/Lion/Attack Logic/AttackState")]
public class LionAttack : EnemyAttackSOBase
{


    //COOLDOWN ATTACKS
    private float attackTimer;
    [SerializeField] private float timeBetweenAttacks;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);

    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        if (!enemy.IsAggreed){
            Debug.Log("funciona");
        }
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();

    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        //A BIT OF COOLDOWN WHEN ATTACK FINISHED
        if (!enemy.isDead)
        {

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
