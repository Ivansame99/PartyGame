using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Torus", menuName = "Enemy Logic/Boss/Attack Logic/Torus")]
public class BossTorusSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform playerTransform;

    [SerializeField] GameObject expansiveWave;

    //Gameobject to instantiate
    private GameObject waveAttack;

    [Header("Wave Attack parameters")]
    [SerializeField] private float waveSpeed;
    [SerializeField] private float waveTimeLife;
    [SerializeField] private int totalWaves;
    private int wavesCounter;

    //COOLDOWN ATTACKS
    [Header("Cooldown between attacks")]
    private float attackTimer;
    [SerializeField] private float attackCooldown;

    private bool isAttacking;
    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = enemy.playerPos;
    }

    public virtual void DoEnterLogic()
    {
        enemy.animator.ResetTrigger("Idle");
    }
    public virtual void DoExitLogic()
    {
        ResetValues();
    }
    public virtual void DoFrameUpdateLogic()
    {
        if (!enemy.isDead)
        {
            if (wavesCounter > totalWaves)
            {
                enemy.stateMachine.ChangeState(enemy.idleState);
            }

            if (attackTimer <= 0)
            {
                enemy.animator.ResetTrigger("Idle");
                enemy.animator.SetTrigger("Torus");
                attackTimer = attackCooldown;
                
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }


        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
    }
    void CreateTorus()
    {
        waveAttack = Instantiate(expansiveWave, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.2f, enemy.transform.position.z), Quaternion.identity);
        Torus torus = waveAttack.GetComponent<Torus>();
        torus.finalDamage = torus.baseDamage + enemy.GetPowerDamageScale(); //cambiar escalado de poder
        torus.SetPushForce(torus.pushForce);
        torus.owner = enemy.gameObject;
        torus.waveSpeed = waveSpeed;
        torus.waveTimeLife = waveTimeLife;
        Destroy(waveAttack, waveTimeLife);
        wavesCounter++;
    }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                CreateTorus();
                break;
        }
    }
    public virtual void ResetValues()
    {
        wavesCounter = 0;
    }
}
