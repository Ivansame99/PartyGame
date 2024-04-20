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

    [SerializeField] private Color color1 = Color.red;
    [SerializeField] private Color color2 = Color.white;

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
        // Vector3 spawnPositionA = enemyUpPos + Random.insideUnitSphere;

        Vector3 spawnPosition = enemy.transform.position; // Obtener la posición del enemigo como posición de origen

        for (int i = 0; i < NumberOfPowerParticles; i++)
        {
            // Agregar un pequeño rango aleatorio al punto de spawn
            Vector3 randomOffset = Random.insideUnitSphere * 0.5f; // Ajusta el valor para controlar el rango
            Vector3 adjustedSpawnPosition = spawnPosition + randomOffset;

            // Calcular una dirección aleatoria hacia arriba y ligeramente hacia un lado
            Vector3 randomDirection = Random.onUnitSphere + Vector3.up * 1f; // Ajusta el valor para cambiar la inclinación lateral
            randomDirection.Normalize(); // Normalizar para asegurarse de que la magnitud sea 1

            // Aplicar una fuerza aleatoria en esa dirección
            float randomForce = Random.Range(PminForce, PmaxForce);
            Vector3 force = randomDirection * randomForce;

            // Instanciar la partícula en la posición ajustada del enemigo
            GameObject powerInstance = Instantiate(PowerPrefab, adjustedSpawnPosition, Quaternion.identity);
            Rigidbody powerRigidbody = powerInstance.GetComponent<Rigidbody>();

            if (powerRigidbody != null)
            {
                powerRigidbody.AddForce(force, ForceMode.Impulse);

                // Elegir aleatoriamente entre color1 y color2
                Color randomColor = Random.value < 0.5f ? color1 : color2;

                // Obtener el renderer para cambiar el color del material
                Renderer renderer = powerInstance.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Asignar el color aleatorio al material
                    renderer.material.color = randomColor;
                }
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
