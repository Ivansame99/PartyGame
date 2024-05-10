using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private float range;
    private int projectilesCounter;

    private GameObject projectile;

    Transform firePoint;
    bool startFireballs;
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
        projectilesCounter = 0;
        enemy.animator.ResetTrigger("Idle");
        enemy.animator.SetTrigger("Fireball");
        startFireballs = false;
    }
    void FireProjectile(Vector3 target)
    {
        projectile = Instantiate(projectilePrefab, target, Quaternion.identity);
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
            //BossRandomMovement();
            if(startFireballs)
            {
                if (projectilesCounter >= totalProjectiles) enemy.stateMachine.ChangeState(enemy.idleState);
                if (attackTimer <= 0 && projectilesCounter <= totalProjectiles)
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
            }

        }else enemy.stateMachine.ChangeState(enemy.deathState);
    }

    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) {
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                enemy.lionAudioManager.RoarAudio();
                break;
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                startFireballs = true;
                break;
        }
    }
    public virtual void ResetValues()
    {

    }
    void BossRandomMovement()
    {
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance ) //done with path
        {
            Vector3 point;
            if (RandomPoint(transform.position, range, out point)) //pass in our centre point and radius of area
            {
                enemy.agent.SetDestination(point);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
