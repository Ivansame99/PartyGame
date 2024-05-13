using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

[CreateAssetMenu(fileName = "Boss Entrance", menuName = "Enemy Logic/Boss/Status Logic/Entrance")]
public class BossEntranceSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;

    protected Transform playerTransform;

    private bool isGrounded;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject fallParticles;
    private Vector3 hitPos;
    private Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = enemy.playerPos;
    }

    public virtual void DoEnterLogic()
    {
        //enemy.animator.ResetTrigger("Entrance");
        enemy.agent.enabled = false;
        enemy.rb.mass = 200;
        //enemy.trailSand.Stop();
    }
    public virtual void DoExitLogic()
    {
        ResetValues();
        enemy.rb.mass = 1;
        //enemy.trailSand.Stop(true);
    }
    public virtual void DoFrameUpdateLogic()
    {
        if (!enemy.isDead)
        {
            Debug.Log(Vector3.Distance(enemy.transform.position, hitPos));
            if (!isGrounded)
            {
                Ray ray = new Ray(transform.position, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)) hitPos = hit.point;

                if (Vector3.Distance(enemy.transform.position, hitPos) <= 1.5f)
                {
                    enemy.animator.SetTrigger("Entrance");
                    Instantiate(fallParticles, hit.point, rotation);
                    isGrounded = true;
                    enemy.agent.enabled = true;
                    
                }
            }
        }
        else enemy.stateMachine.ChangeState(enemy.deathState);
    }

    public virtual void DoPhysicsLogic() { }
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.EnemyAttackFinished:
                enemy.stateMachine.ChangeState(enemy.idleState);
                break;
        }
    }
    public virtual void ResetValues()
    {
        
    }

}
