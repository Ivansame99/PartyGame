using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpecialAttack", menuName = "Enemy Logic/Secutor/Attack Logic/Secutor Special Attack")]
public class SecutorSpecialAttack : EnemySpecialAttackSOBase
{
    [SerializeField] private ParticleSystem areaAttackParticles;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                Instantiate(areaAttackParticles, enemy.transform.position, Quaternion.identity);
                break;
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                Debug.Log("Attack Finished");
                
                break;
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetInteger("AnimationType", 3);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
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
