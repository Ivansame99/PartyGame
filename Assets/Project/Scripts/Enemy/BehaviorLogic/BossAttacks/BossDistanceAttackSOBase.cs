using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Projectile", menuName = "Enemy Logic/Boss/Attack Logic/Projectile")]
public class BossDistanceAttackSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Transform playerTransform;

    [Header("Projectile parameters")]
    [SerializeField] private GameObject projectilePrefab;

     
    private float attackTimer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int totalProjectiles;
    private int projectilesCounter;

    private GameObject projectile;

    Transform firePoint;
    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = enemy.playerPos;
    }

    public virtual void DoEnterLogic()
    {
        attackTimer = attackCooldown;
  
    }
    void FireProjectile(Vector3 target)
    {
        projectile = Instantiate(projectilePrefab, target, Quaternion.identity);
        projectile.GetComponent<EnemyDamage>().enemy = enemy;
    }

    void ThrowBottle(Vector3 target)
    {
        firePoint.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 10f, enemy.transform.position.z);
        projectile = Instantiate(projectilePrefab, target, Quaternion.identity);
        projectile.GetComponent<DrunkProjectile>().finalPosition = enemy.transform;
        projectile.GetComponent<DrunkProjectile>().firePoint = enemy.transform;
        projectile.GetComponent<EnemyDamage>().enemy = enemy;
    }
    public virtual void DoExitLogic()
    {
        ResetValues();
    }
    public virtual void DoFrameUpdateLogic()
    {
        if (!enemy.isDead)
        {
            if(attackTimer <= 0 && projectilesCounter <= totalProjectiles)
            {
                for (int i = 0; i < enemy.enemyDirector.players.Count; i++)
                {
                    Vector3 target = new Vector3(enemy.enemyDirector.players[i].transform.position.x, enemy.enemyDirector.players[i].transform.position.y + 20f, enemy.enemyDirector.players[i].transform.position.z);
                    FireProjectile(target);
                }
                attackTimer = attackCooldown;
                projectilesCounter++;
            }
            else attackTimer -= Time.deltaTime;
        }else enemy.stateMachine.ChangeState(enemy.deathState);
    }

    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValues()
    {

    }
}
