using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Entrance", menuName = "Enemy Logic/Boss/Status Logic/Entrance")]
public class BossEntranceSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform playerTransform;

    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = enemy.playerPos;
    }

    public virtual void DoEnterLogic()
    {
        //enemy.animator.ResetTrigger("Idle");
        Debug.Log("Boss Entrance");

    }
    public virtual void DoExitLogic()
    {
        ResetValues();
    }
    public virtual void DoFrameUpdateLogic()
    {
        if (!enemy.isDead)
        {

        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
    }

    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        switch (triggerType)
        {

        }
    }
    public virtual void ResetValues()
    {
        
    }
}
