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

    private Animator healthBarAnimator;

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

    public Material URPMaterial;

    // Textura original y textura de parpadeo
    // private Texture originalBaseMap;
    public Texture baseMapParpadeo;
    public Texture baseMapOriginal;
    // Duraci�n del parpadeo
    // public float delayParpadeo = 0.5f;


    private GameObject playerUI;
    private HealthBarController playerUIHealth;
    private Animator playerUIHealthAnimator;
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
        // originalBaseMap = URPMaterial.GetTexture("_BaseMap");
        URPMaterial.SetTexture("_BaseMap", baseMapOriginal);
        pushBack = false;
        healthBarAnimator = healthBar.gameObject.GetComponent<Animator>();
        playerUI = GameObject.FindGameObjectWithTag("UI" + this.gameObject.name);
        playerUIHealthAnimator = playerUI.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        playerUIHealth = playerUI.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<HealthBarController>();
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
        StartCoroutine(RedEffect());
        healthBarAnimator.SetTrigger("Damage");
        playerUIHealthAnimator.SetTrigger("Damage");
        health -= damage;
        if (healthBarC != null)
        {
            playerUIHealth.SetProgress(health / maxHealth, 2);
            healthBarC.SetProgress(health / maxHealth, 2);
        }
        if (health <= 0) Die();
        playerController.invencibilityTimer = inmuneTime;
    }
    IEnumerator RedEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            URPMaterial.SetTexture("_BaseMap", baseMapParpadeo);
            yield return new WaitForSeconds(0.1f);
            URPMaterial.SetTexture("_BaseMap", baseMapOriginal);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Die()
    {
        anim.SetTrigger("Death");
        playersRespawn.NotifyDead();
        respawnTimer = respawnCD;
        currentPower = GetComponent<PowerController>().GetCurrentPowerLevel()/2;
        //Debug.Log(lastAttacker);
        if (lastAttacker != null) lastAttacker.GetComponent<PowerController>().SetCurrentPowerLevel(currentPower); //Se le suma la puntuacion del enemigo
        GetComponent<PowerController>().OnDieSetCurrentPowerLevel();
        DisablePlayer();
        dead = true;
        deathTimer = deathCD;
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
        powerController.enabled = true;
        playerController.enabled = true;
        playerController.dodge = false; //Por si estaba rodando cuando murio
        //powerController.SetCurrentPowerLevel(powerController.GetCurrentPowerLevel()/2);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(true);
        powerBar.gameObject.SetActive(true);
        dead = false;
        deadAux = false;
        restart = false;
        health = maxHealth;
        playerController.invencibilityTimer = 0.5f;
        healthBarC.SetProgress(health / maxHealth, 2);
        playerUIHealth.SetProgress(health / maxHealth, 2);
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

            lastAttacker = other.transform.parent.parent.gameObject;
            SlashController slashController = other.GetComponent<SlashController>();
            attackPosition = other.gameObject.transform.position;
            pushBack = true;
            pushForce = slashController.pushForce;
            ReceiveDamage(slashController.finalDamage);
        }

        if (other.gameObject.tag == "Potion")
        {
            health += 50;
            healthBarC.SetProgress(health / maxHealth, 2);
            playerUIHealth.SetProgress(health / maxHealth, 2);
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
                lastAttacker = ac.owner;
                attackPosition = collision.gameObject.transform.position;
                pushBack = true;
                pushForce = ac.pushForce;

                ReceiveDamage(ac.finalDamage);
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
