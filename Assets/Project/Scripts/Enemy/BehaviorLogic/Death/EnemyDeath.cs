using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDeath", menuName = "Enemy Logic/General/Enemy Death")]
public class EnemyDeath : EnemyDeathSOBase
{
	[SerializeField] private HelmetPrefab[] helmetPrefabs;
	[SerializeField] private float minForce = 20f;
	[SerializeField] private float maxForce = 55f;
    [SerializeField] private float PminForce = 7f;
    [SerializeField] private float PmaxForce = 12f;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float deathTimer;
    [SerializeField] private GameObject PowerPrefab;

    private Vector3 scale = new Vector3(1, 1, 1);
	private int powerPerParticle;
	private int numberOfPowerParticles;

    private Vector3 spawnPosition;
    private bool deathFlag = false;

	private string diePath = "event:/SFX/Enemies/Secutor/Die";

	public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.secutorAudioManager.PlayDeath();
        enemy.animator.SetTrigger("Die");
		FMODUnity.RuntimeManager.PlayOneShot(diePath);
	}

    void StopParticleLoop(ParticleSystem particleSystemInstance)
    {
        // Detener el sistema de partículas
        ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
        }

        // Desvincular las partículas del enemigo
        particleSystemInstance.transform.SetParent(null);
    }
    void CalculateNumberOfParticles()
    {
        int powerToCalculate = enemy.powerController.GetHalfPowerLevel();
       
        if (powerToCalculate >= 0 && powerToCalculate <= 10) // ENTRE O Y 20 DE FUERZA
        {
            numberOfPowerParticles = 2;
            scale = new Vector3(0.5f, 0.5f, 0.5f);

        }
        else if (powerToCalculate >= 11 && powerToCalculate <= 25) // ENTRE 20 Y 50
        {
            numberOfPowerParticles = 5;
            scale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        else if (powerToCalculate >= 26 && powerToCalculate <= 50) // ENTRE 50 Y 100
        {
            numberOfPowerParticles = 10;
            scale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else if (powerToCalculate >= 51 && powerToCalculate <= 150) // ENTRE 100 Y 300
        {
            numberOfPowerParticles = 15;
            scale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else if (powerToCalculate >= 151 && powerToCalculate <= 500) // ENTRE 300 Y 1000
        {
            numberOfPowerParticles = 20;
            scale = new Vector3(0.9f, 0.9f, 0.9f);

        }
        else if (powerToCalculate >= 501) // MAS DE 1000
        {
            numberOfPowerParticles = 25;
            scale = new Vector3(1f, 1f, 1f);

        }

        if (numberOfPowerParticles!=0) powerPerParticle = powerToCalculate / numberOfPowerParticles; //DIVIDES LA MITAD DEL PODER(LO QUE TIENES QUE REPARTIR) ENTRE EL NUMERO DE PARTICULAS QUE SUELTAN, POR LO QUE CADA PARTICULA TIENE SU PODER




    }
    void Death()
    {
        deathFlag = true;
        //Feedback
        StopParticleLoop(enemy.trailSand);

        if(enemy.playerPos != null)
        {
			enemy.enemyTargetController.DecreasePlayerTarget(enemy.playerPos.name);
			spawnPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 4f, enemy.transform.position.z);
		}
            
        CalculateNumberOfParticles();

        for (int i = 0; i < numberOfPowerParticles; i++)
        {
            Vector3 adjustedSpawnPosition = spawnPosition;
            GameObject powerInstance = Instantiate(PowerPrefab,adjustedSpawnPosition, Quaternion.identity);
            powerInstance.GetComponent<PowerParticleController>().SetPowerAmount(powerPerParticle);
            Rigidbody powerRigidbody = powerInstance.GetComponent<Rigidbody>();

            if (powerRigidbody != null)
            {
                powerInstance.transform.localScale = scale;
                powerRigidbody.AddForce(new Vector3(Random.Range(-0.3f, 0.3f), 0.3f, Random.Range(-0.3f, 0.3f)), ForceMode.Impulse);
            }
        }
        foreach (var helmetPrefab in helmetPrefabs)
		{
			if (Random.value <= helmetPrefab.spawnChance)
			{
				
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
        if (enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && enemy.animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            if(!deathFlag) Death();
        }
        if (deathTimer <= 0)
        {
            if(!deathFlag) Death();
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
