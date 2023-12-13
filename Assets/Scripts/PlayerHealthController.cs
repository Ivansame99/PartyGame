using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    [SerializeField]
    private Transform powerBar;

    private float timer = 0;

    //Variables que iran donde se spawneen los pjs

    private Canvas healBarCanvas;

    [SerializeField]
    private Camera camera;

    private PlayerController playerController;
    private PowerController powerController;
    private CapsuleCollider playerCollider;
    //private CapsuleCollider playerCollider;

    private Animator anim;

    private float deathCD = 2;
    private float deathTimer = 0;

    public bool dead = false;
    private bool deadAux = false;
    private bool restart = false;

    private float respawnTimer;

    [SerializeField]
    private float respawnCD;

    private GameObject lastAttacker;
    private float currentPower;

    private PlayersRespawn playersRespawn;

    private bool pushBack;
    private Vector3 attackPosition;
    private float pushForce;

    [SerializeField]
    private GameObject cross1, cross2, glow;

    public Material materialConURP;

    // Textura original y textura de parpadeo
    private Texture originalBaseMap;
    public Texture baseMapParpadeo;
    public Texture baseMapOriginal;
    // Duración del parpadeo
    public float duracionParpadeo = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        healBarCanvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        SetupHealthBar(healBarCanvas, camera);
        SetupStaminaBar(healBarCanvas, camera);
        health = maxHealth;
        playerController = GetComponent<PlayerController>();
        powerController = GetComponent<PowerController>();
        playerCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        playersRespawn = FindObjectOfType<PlayersRespawn>();
        pushBack = false;
        originalBaseMap = materialConURP.GetTexture("_BaseMap");
        materialConURP.SetTexture("_BaseMap", baseMapOriginal);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerController.invencibility);

        if (deathTimer >= 0)
        {
            deathTimer -= Time.deltaTime;
        }
        else if (dead == true && !deadAux)
        {
            deadAux = true;
            StartCoroutine(ScaleUpAndDown(this.transform, new Vector3(0f, 0f, 0f), 1f));
        }

        if (restart) Respawn();
    }

    private void FixedUpdate()
    {
        if (pushBack)
        {
            Vector3 direction = (this.transform.position - attackPosition).normalized;
            direction.y = 0;
            this.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForce, ForceMode.Impulse);
            pushBack = false;
        }
    }

    public void ReceiveDamage(float damage)
    {
        StartCoroutine(ParpadearRojo());
        health -= damage;
        timer = inmuneTime;
        if (healthBarC != null) healthBarC.SetProgress(health / maxHealth, 2);
        if (health <= 0) Die();
        playerController.invencibilityTimer = 0.5f;
    }
    IEnumerator ParpadearRojo()
    {
        for (int i = 0; i < 5; i++)
        {
            materialConURP.SetTexture("_BaseMap", baseMapParpadeo);
            yield return new WaitForSeconds(0.1f);
            materialConURP.SetTexture("_BaseMap", baseMapOriginal);
            yield return new WaitForSeconds(0.1f);
        }
    }
        void Die()
    {
        anim.SetTrigger("Death");
        deathTimer = deathCD;
        dead = true;
        playersRespawn.NotifyDead();
        respawnTimer = respawnCD;
        GetComponent<PowerController>().OnDieSetCurrentPowerLevel();
        currentPower = GetComponent<PowerController>().GetCurrentPowerLevel();
        //Debug.Log(currentPower);
        lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower); //Se le suma la puntuacion del enemigo
        DisablePlayer();
        /*float destroyDelay = Random.value;
        Destroy(this.gameObject, destroyDelay);
        Destroy(healthBar.gameObject, destroyDelay);*/
        //anim.SetBool("Death", false);
    }

    void DisablePlayer()
    {
        playerController.enabled = false;
        powerController.enabled = false;
        healthBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(false);
        powerBar.gameObject.SetActive(false);
        //playerCollider.enabled = false;
    }

    public void EnablePlayer()
    {
        playerController.enabled = true;
        playerController.dodge = false; //Por si estaba rodando cuando murio
        powerController.enabled = true;
        //powerController.SetCurrentPowerLevel(powerController.GetCurrentPowerLevel()/2);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(true);
        powerBar.gameObject.SetActive(true);
        dead = false;
        deadAux = false;
        restart = false;
        health = maxHealth;
        timer = inmuneTime;
        healthBarC.SetProgress(health / maxHealth, 2);
        playerController.ResetStamina();
        //anim.enabled = true;
    }

    void Respawn()
    {
        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;
        }
        else
        {
            //this.transform.position = Vector3.zero;
            playersRespawn.SpawnPlayer(this.gameObject);
            //EnablePlayer();
        }
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SlashEffect") && !playerController.invencibility && !dead)
        {
            cross1.SetActive(false);
            cross2.SetActive(false);
            glow.SetActive(false);


            cross1.SetActive(true);
            cross2.SetActive(true);
            glow.SetActive(true);

            SlashController slashController = other.GetComponent<SlashController>();
            attackPosition = other.gameObject.transform.position;
            pushBack = true;
            pushForce = slashController.pushForce;
            ReceiveDamage(slashController.finalDamage);
            lastAttacker = other.transform.parent.parent.gameObject;
        }

        if (other.gameObject.tag == "Potion")
        {
            health += 50;
            healthBarC.SetProgress(health / maxHealth, 2);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow" && !playerController.invencibility && !dead)
        {
            ArrowController ac = collision.gameObject.GetComponent<ArrowController>();
            if (this.gameObject == ac.owner && ac.invencibilityTimerOnSpawnOwner > 0)
            {
                //Se pega contra si mismo al principio, no hace nada
            }
            else
            {
                attackPosition = collision.gameObject.transform.position;
                pushBack = true;
                pushForce = ac.pushForce;

                ReceiveDamage(ac.finalDamage);
                lastAttacker = ac.owner;
                Destroy(collision.gameObject);
            }
        }
    }

    IEnumerator ScaleUpAndDown(Transform transform, Vector3 upScale, float duration)
    {
        Vector3 initialScale = transform.localScale;

        for (float time = 0; time < duration * 2; time += Time.deltaTime)
        {
            //float progress = Mathf.PingPong(time, duration) / duration;
            transform.localScale = Vector3.Lerp(initialScale, upScale, time);
            yield return null;
        }
        restart = true;
        anim.SetTrigger("Respawn");
        //Respawn();
        //this.transform.position = 
        //transform.localScale = initialScale;
    }
}
