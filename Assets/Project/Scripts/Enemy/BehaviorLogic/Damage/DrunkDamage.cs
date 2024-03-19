using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Take Damage", menuName = "Enemy Logic/Drunk/Status Logic/Damage")]
public class DrunkDamage : EnemyDamageSOBase
{
    private float stunedTimer;
    [SerializeField] private float stunTime;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Damage");
        enemy.drunkAudioManager.PlayDamage();
        stunedTimer = stunTime;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.rb.velocity = Vector3.zero;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            if (stunedTimer <= 0)
            {
                enemy.SetDamagedStatus(false);
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }
            else
            {
                stunedTimer -= Time.deltaTime;
            }
        }else enemy.stateMachine.ChangeState(enemy.deathState);

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
