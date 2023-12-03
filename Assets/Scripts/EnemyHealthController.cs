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
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Cross1,Cross2,Glow;

    void Start()
    {
        healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
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
       // Cross1.SetActive(true);
       // Cross2.SetActive(true);
      //  Glow.SetActive(true);
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
            Cross1.SetActive(false);
            Cross2.SetActive(false);
            Glow.SetActive(false);

           
            Cross1.SetActive(true);
            Cross2.SetActive(true);
            Glow.SetActive(true);

            ReceiveDamage(15);
        }
    }
}
