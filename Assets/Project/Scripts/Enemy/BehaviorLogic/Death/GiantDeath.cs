using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Giant Death", menuName = "Enemy Logic/Giant/Status Logic/Enemy Death")]
public class GiantDeath : EnemyDeathSOBase
{
    [SerializeField] private HelmetPrefab[] helmetPrefabs;
    [SerializeField] private float minForce = 2f;
    [SerializeField] private float maxForce = 5f;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private float deathTimer;
    [SerializeField] private int powerPerParticle;
    [SerializeField] private int numberOfPowerParticles;
    [SerializeField] private GameObject PowerPrefab;

    [SerializeField] private Color color1 = Color.red;
    [SerializeField] private Color color2 = Color.white;

    private Vector3 scale = new Vector3(1, 1, 1);

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        enemy.giantAudioManager.PlayDeath();
        enemy.animator.SetTrigger("Die");
        //enemy.secutorAudioManager.PlayDeath();
    }
    void StopParticleLoop(ParticleSystem particleSystemInstance)
    {
        Debug.Log("Entra");
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

        if (numberOfPowerParticles != 0) powerPerParticle = powerToCalculate / numberOfPowerParticles; //DIVIDES LA MITAD DEL PODER(LO QUE TIENES QUE REPARTIR) ENTRE EL NUMERO DE PARTICULAS QUE SUELTAN, POR LO QUE CADA PARTICULA TIENE SU PODER




    }

    void Death()
    {
        //Feedback
        StopParticleLoop(enemy.trailSand);
        enemy.enemyTargetController.DecreasePlayerTarget(enemy.playerPos.name);

        Vector3 spawnPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 4f, enemy.transform.position.z);
        CalculateNumberOfParticles();

        for (int i = 0; i < numberOfPowerParticles; i++)
        {
            Vector3 adjustedSpawnPosition = spawnPosition;
            GameObject powerInstance = Instantiate(PowerPrefab, adjustedSpawnPosition, Quaternion.identity);
            powerInstance.GetComponent<PowerParticleController>().SetPowerAmount(powerPerParticle);
            Rigidbody powerRigidbody = powerInstance.GetComponent<Rigidbody>();

            if (powerRigidbody != null)
            {
                powerInstance.transform.localScale = scale;
                powerRigidbody.AddForce(new Vector3(Random.Range(-20f, 20f), 10, Random.Range(-20f, 20f)), ForceMode.Impulse);
                Color randomColor = Random.value < 0.5f ? color1 : color2;

                Renderer renderer = powerInstance.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = randomColor;
                }
            }
        }


        foreach (var helmetPrefab in helmetPrefabs)
        {
            if (Random.value <= helmetPrefab.spawnChance)
            {
                float yOffset = 2f;
                Vector3 enemyUpPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + yOffset, enemy.transform.position.z);
               // Vector3 spawnPosition = enemyUpPos + Random.insideUnitSphere;
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
