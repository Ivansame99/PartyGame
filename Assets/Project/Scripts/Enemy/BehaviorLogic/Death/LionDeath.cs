using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Lion Death", menuName = "Enemy Logic/Boss/Lion/Satus Logic/Enemy Death")]
public class LionDeath : EnemyDeathSOBase
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float deathTimer;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        //enemy.animator.SetTrigger("Death");
        //enemy.secutorAudioManager.PlayDeath();
    }
    void StopParticleLoop(ParticleSystem particleSystemInstance)
    {
        // Detener el sistema de partículas
        ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Debug.Log("Funciona");
            ps.Stop();
        }

        // Desvincular las partículas del enemigo
        particleSystemInstance.transform.SetParent(null);
    }
    void Death()
    {
        //Feedback
        StopParticleLoop(enemy.trailSand);
        enemy.enemyTargetController.DecreasePlayerTarget(enemy.playerPos.name);

        Instantiate(deathParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            Death();
        }
        if (deathTimer <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            deathTimer -= Time.deltaTime;
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
