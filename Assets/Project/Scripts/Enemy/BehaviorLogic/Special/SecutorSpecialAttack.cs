using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpecialAttack", menuName = "Enemy Logic/Secutor/Attack Logic/Secutor Special Attack")]
public class SecutorSpecialAttack : EnemySpecialAttackSOBase
{
    [SerializeField] private ParticleSystem areaAttackParticles;
    [SerializeField] private float scaleAttackParticles;
    private Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
    private Vector3 scale = new Vector3(4f, 4f, 4f);
    private bool changeState;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttack:
                CreateParticles();
                Debug.Log("ataca");
                break;
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                Stunned();
                Debug.Log("finaliza");
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
        if(enemy.isDead)
        {
            enemy.stateMachine.ChangeState(enemy.deathState);
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
    private void CreateParticles()
    {
        ParticleSystem instantiatedObject = Instantiate(areaAttackParticles, enemy.transform.position, rotation);
        instantiatedObject.transform.localScale = scale;
    }
    private void Stunned()
    {
        enemy.stateMachine.ChangeState(enemy.stunnedState);
    }

}
