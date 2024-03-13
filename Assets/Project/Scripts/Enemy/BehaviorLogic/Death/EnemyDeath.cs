using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDeath", menuName = "Enemy Logic/General/Enemy Death")]
public class EnemyDeath : EnemyDeathSOBase
{
	[SerializeField] private HelmetPrefab[] helmetPrefabs;
	[SerializeField] private float minForce = 2f;
	[SerializeField] private float maxForce = 5f;
	[SerializeField] private GameObject deathParticles;

	public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        switch (triggerType)
        {
            case Enemy.AnimationTriggerType.Death:
                Death();
                break;
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.animator.SetTrigger("Die");
    }

    void Death()
    {
        //Feedback
        enemy.enemyTargetController.DecreasePlayerTarget(enemy.playerPos.name);
		foreach (var helmetPrefab in helmetPrefabs)
		{
			if (Random.value <= helmetPrefab.spawnChance)
			{
				float yOffset = 2f;
				Vector3 enemyUpPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + yOffset, enemy.transform.position.z);
				Vector3 spawnPosition = enemyUpPos + Random.insideUnitSphere;
				GameObject helmetInstance = Instantiate(helmetPrefab.prefab, spawnPosition, Quaternion.identity);
				Rigidbody helmetRigidbody = helmetInstance.GetComponent<Rigidbody>();
				if (helmetRigidbody != null)
				{
					Vector3 randomDirection = Random.onUnitSphere;
					float randomForce = Random.Range(minForce, maxForce);
					helmetRigidbody.AddForce(randomDirection * randomForce, ForceMode.Impulse);
				}
			}
		}
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
