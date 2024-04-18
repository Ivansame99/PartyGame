using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDeath", menuName = "Enemy Logic/General/Enemy Death")]
public class EnemyDeath : EnemyDeathSOBase
{
	[SerializeField] private HelmetPrefab[] helmetPrefabs;
	[SerializeField] private float minForce = 2f;
	[SerializeField] private float maxForce = 5f;
    [SerializeField] private float PminForce = 0.1f;
    [SerializeField] private float PmaxForce = 0.3f;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float deathTimer;
    [SerializeField] private float NumberOfPowerParticles;
    [SerializeField] private GameObject PowerPrefab;



    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.secutorAudioManager.PlayDeath();
        enemy.animator.SetTrigger("Die");
    }

    void StopParticleLoop(ParticleSystem particleSystemInstance)
    {
        // Detener el sistema de part�culas
        ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Debug.Log("Funciona");
            ps.Stop();
        }

        // Desvincular las part�culas del enemigo
        particleSystemInstance.transform.SetParent(null);
    }

    void Death()
    {
        //Feedback
        StopParticleLoop(enemy.trailSand);

        enemy.enemyTargetController.DecreasePlayerTarget(enemy.playerPos.name);

        float yOffset = 2f; // Altura inicial sobre el enemigo
        Vector3 enemyUpPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + yOffset, enemy.transform.position.z);
        
        Vector3 spawnPositionA = enemyUpPos + Random.insideUnitSphere;

        for (int i = 0; i < NumberOfPowerParticles; i++)
        {
           
            Vector3 spawnPosition = enemyUpPos + Random.insideUnitSphere * 1.5f; 

          
            GameObject powerInstance = Instantiate(PowerPrefab, spawnPosition, Quaternion.identity);
            Rigidbody powerRigidbody = powerInstance.GetComponent<Rigidbody>();

            if (powerRigidbody != null)
            {
               
                Vector3 randomDirection = Random.onUnitSphere + Vector3.up * 20; 
                randomDirection.Normalize(); 

               
                float randomForce = Random.Range(PminForce, PmaxForce);
                Vector3 force = randomDirection * randomForce;

            
                powerRigidbody.AddForce(force, ForceMode.Impulse);
            }
        }
        foreach (var helmetPrefab in helmetPrefabs)
		{
			if (Random.value <= helmetPrefab.spawnChance)
			{
				
				GameObject helmetInstance = Instantiate(helmetPrefab.prefab, spawnPositionA, Quaternion.identity);
             
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
