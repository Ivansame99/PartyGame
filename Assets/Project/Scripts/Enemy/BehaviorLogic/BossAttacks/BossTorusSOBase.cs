using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Torus", menuName = "Enemy Logic/Boss/Attack Logic/Torus")]
public class BossTorusSOBase : ScriptableObject
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

    public virtual void DoEnterLogic() { 
        Debug.Log("DoEnterLogic");
    }
    public virtual void DoExitLogic()
    {
        ResetValues();
    }
    public virtual void DoFrameUpdateLogic()
    {

    }
    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }
    public virtual void ResetValues() { }
}
