using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Drunk Attack", menuName = "Enemy Logic/Drunk/Attack Logic/Attack")]
public class DrunkAttack : EnemyAttackSOBase
{
    [SerializeField] private GameObject bottlePrefab;
    private GameObject bottle;

    //COOLDOWN ATTACKS
    private float attackTimer;
    [SerializeField] private float attackCooldown;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        
        //Vector3 enemyPosition = new Vector3(enemy.transform.position.x,enemy.transform.position.y + 4f,enemy.transform.position.z);

        // Instancia la botella en la posición del enemigo

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
            if (!enemy.IsAggreed)
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }

            if (attackTimer <= 0)
            {
                attackTimer = attackCooldown;
                bottle = Instantiate(bottlePrefab, enemy.transform.position, Quaternion.identity);
                bottle.GetComponent<DrunkProjectile>().finalPosition = enemy.playerPos;
                bottle.GetComponent<DrunkProjectile>().firePoint = enemy.transform;
                bottle.GetComponent<DrunkProjectile>().enemy = enemy;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);

        }
    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
        if(bottlePrefab != null)
        {
        }
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
