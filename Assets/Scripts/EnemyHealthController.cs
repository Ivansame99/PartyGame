using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class EnemyHealthController : MonoBehaviour
{
   
    private float health;

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float inmuneTime;

    [SerializeField]
    private HealthBarController healthBarC;

    [SerializeField]
    private Transform healthBar;

    private float timer;

    //Variables que iran donde se spawneen los pjs
    private Canvas healBarCanvas;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject powerLevelGameObject;
    private float currentPower;

    private GameObject lastAttacker;

    // Start is called before the first frame update
    void Start()
    {
        healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupHealthBar(healBarCanvas, camera);
        health=maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentPower = GetComponent<PowerController>().GetCurrentPowerLevel();
        if (timer >= 0)
        {
            timer-=Time.deltaTime;
        }
    }

    public void ReceiveDamage(float damage)
    {
        if (timer <= 0)
        {   
            health -= damage;
            timer = inmuneTime;
            healthBarC.SetProgress(health / maxHealth, 5f);
            if (health <= 0) Die();
        }
    }

    void Die()
    {
        /*float destroyDelay = Random.value;
        Destroy(this.gameObject, destroyDelay);
        Destroy(healthBar.gameObject, destroyDelay);*/
        lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower); //Se le suma la puntuacion del enemigo
        Destroy(healthBar.gameObject);
        Destroy(powerLevelGameObject.gameObject);
        Destroy(this.gameObject);
        
    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        healthBar.transform.SetParent(canvas.transform);
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlashEffect"))
        {
            ReceiveDamage(15);
            lastAttacker = other.transform.parent.parent.gameObject;
            //Debug.Log(lastAttacker);
        }
    }
}
