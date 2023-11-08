using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField]
    private Canvas healBarCanvas;

    [SerializeField]
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        SetupHealthBar(healBarCanvas, camera);
        health=maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
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
            //Debug.Log(health);
            if (health <= 0) Die();
        }
    }

    void Die()
    {
        /*float destroyDelay = Random.value;
        Destroy(this.gameObject, destroyDelay);
        Destroy(healthBar.gameObject, destroyDelay);*/
        Destroy(this.gameObject);
        Destroy(healthBar.gameObject);
    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        healthBar.transform.SetParent(canvas.transform);
        /*if(healthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            faceCamera.camera = camera;
        }*/
    }
     private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SlashEffect"))
        {
            ReceiveDamage(15);
        }
    }
}
