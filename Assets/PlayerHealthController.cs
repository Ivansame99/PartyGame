using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
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

    [SerializeField]
    private Transform staminaBar;

    private float timer;

    //Variables que iran donde se spawneen los pjs

    private Canvas healBarCanvas;

    [SerializeField]
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupHealthBar(healBarCanvas, camera);
        SetupStaminaBar(healBarCanvas, camera);
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void ReceiveDamage(float damage)
    {
        if (timer <= 0)
        {
            health -= damage;
            timer = inmuneTime;
            healthBarC.SetProgress(health / maxHealth, 2);
            //Debug.Log(health);
            if (health <= 0) Die();
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

    public void SetupStaminaBar(Canvas canvas, Camera camera)
    {
        staminaBar.transform.SetParent(canvas.transform);
        /*if(healthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            faceCamera.camera = camera;
        }*/
    }
}
