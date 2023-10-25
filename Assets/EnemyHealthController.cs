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
    private HealthBarController healthBar;

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

        if (health <= 0) Die();
    }

    public void ReceiveDamage(float damage)
    {
        if (timer <= 0)
        {
            healthBar.SetProgress(health/maxHealth, 3);
            health -= damage;
            timer = inmuneTime;
            Debug.Log(health);
        }
    }

    void Die()
    {
        float destroyDelay = Random.value;
        Destroy(this.gameObject, destroyDelay);
        Destroy(healthBar.gameObject, destroyDelay);
    }

    public void SetupHealthBar(Canvas canvas, Camera camera)
    {
        healthBar.transform.SetParent(canvas.transform);
        /*if(healthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            faceCamera.camera = camera;
        }*/
    }
}
