using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySecutorStuned", menuName = "Enemy Logic/Secutor/Status Logic/Stuned")]
public class SecutorStuned : EnemyStunedSOBase
{

    //COOLDOWN STUNED
    private float stunedTimer;
    [SerializeField] private float stunTime;
    [SerializeField] private GameObject starStun;
    [SerializeField] private float stunYPosition;
    private GameObject starStunClone;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetBool("Stun", true);
        stunedTimer = stunTime;
        starStunClone = Instantiate(starStun, new Vector3(enemy.transform.position.x, enemy.transform.position.y + stunYPosition, enemy.transform.position.z), starStun.transform.rotation, this.enemy.transform);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        enemy.animator.SetBool("Stun", false);
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if(!enemy.isDead)
        {
            if (stunedTimer <= 0)
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
                Destroy(starStunClone);
            }
            else
            {
                stunedTimer -= Time.deltaTime;
            }
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
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


