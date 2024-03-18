using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Attack", menuName = "Enemy Logic/Drunk/Attack Logic/Attack")]
public class DrunkAttack : EnemyAttackSOBase
{
    [SerializeField] private GameObject bottlePrefab;
    private GameObject bottle;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Attack");
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isDead)
        {
            enemy.rb.velocity = Vector3.zero;
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                ThrowBottle();
                enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
    }
    void ThrowBottle()
    {
        bottle = Instantiate(bottlePrefab, enemy.transform.position, Quaternion.identity);
        bottle.GetComponent<DrunkProjectile>().finalPosition = enemy.playerPos;
        bottle.GetComponent<DrunkProjectile>().firePoint = enemy.transform;
        bottle.GetComponent<DrunkProjectile>().enemy = enemy;
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
