using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] private float normalAttackSpeed;
    [SerializeField] private float specialAttackSpeed;
    [SerializeField] private float enemyBaseDamage;
    [SerializeField] private float enemyBasePushForce;
    [SerializeField] private float damagePushForce;
    private Rigidbody rb;
    private Animator animator;
    private bool onlyOnceSpecial,onlyOnceAttack,onlyOnceDamaged;
    NavMeshAgent agent;

    //SLASH STUFF
    public GameObject boundCharacter;
    public GameObject SlashEffect;
    private PowerController powerController;
    [SerializeField]
    private Transform slashDirection;

    [SerializeField]
    private GameObject slashParticle;
    [SerializeField] 
    private GameObject bigSlashParticleSystem;

    private SlashController slashControllerBig;

    [SerializeField]
    private GameObject slashCollider;
    [SerializeField]
    private GameObject bigSlashCollider;
    private SlashController slashController;
    private ParticleSystem slashParticleSystem;
    private Vector3 evadeAttackDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        evadeAttackDirection = -transform.forward;
        onlyOnceSpecial = true;
        onlyOnceAttack = true;
        slashParticleSystem = slashParticle.GetComponent<ParticleSystem>();
        slashController = slashCollider.GetComponent<SlashController>();
        slashControllerBig = bigSlashCollider.GetComponent<SlashController>();
        powerController = GetComponent<PowerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (animator.GetBool("attackOn"))
        {
            //mete slash normal (SI NO FUNCIONA AQUI, METELO EN LA FUNCION DE ABAJO SLASH
            //  Slash();
            //navMeshAgent.updatePosition = false;
            //navMeshAgent.updateRotation = false;
            agent.enabled = false;
            rb.MovePosition(transform.position + transform.forward * normalAttackSpeed * Time.fixedDeltaTime);
            onlyOnceAttack = false;
            //navMeshAgent.isStopped = true;
        }
        
        if (!animator.GetBool("attackOn") && !onlyOnceAttack)
        {
            agent.enabled = true;
            onlyOnceAttack = true;
            slashCollider.SetActive(false);
            slashParticle.SetActive(false);

            animator.SetTrigger("attackFinished");
            //navMeshAgent.isStopped = false;
        }
        
        if (animator.GetBool("isEvading"))
        {
            //navMeshAgent.enabled = false;
            //METE SLASH ESPECIAL
            bigSlashParticleSystem.SetActive(true);
            bigSlashCollider.SetActive(true);
            slashControllerBig.finalDamage = enemyBaseDamage/3 + powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
            slashControllerBig.pushForce = enemyBasePushForce;
            //rb.MovePosition(transform.position + evadeAttackDirection * normalAttackSpeed * Time.fixedDeltaTime);
            onlyOnceSpecial = false;
        }
        if (!animator.GetBool("isEvading") && !onlyOnceSpecial)
        {
            bigSlashParticleSystem.SetActive(false);
            bigSlashCollider.SetActive(false);
            onlyOnceSpecial = true;
        }

        if (animator.GetBool("isDamaged"))
        {
            agent.enabled = false;
            rb.MovePosition(transform.position + -transform.forward * damagePushForce * Time.fixedDeltaTime);
            onlyOnceDamaged = false;
            //navMeshAgent.isStopped = true;
        }

        if (!animator.GetBool("isDamaged") && !onlyOnceDamaged)
        {
            agent.enabled = true;
            onlyOnceDamaged = true;
            //navMeshAgent.isStopped = false;
        }

    }

    public void Slash()
{
        //Cosas de slash
        Vector3 savedPosition = slashDirection.position;
        slashParticle.transform.position = savedPosition;
        slashCollider.transform.position = savedPosition;

        Quaternion lookRotation = slashDirection.rotation; // Usamos directamente la rotación del objeto
        slashCollider.SetActive(false);
        slashParticle.SetActive(false);

        var mainModule = slashParticleSystem.main;

        float newAngle = lookRotation.eulerAngles.y;
        newAngle = Mathf.Repeat(newAngle, 360f);
        int angleInt = Mathf.FloorToInt(newAngle);

        // LO DE ABAJO FUNCIONA PERO HAY QUE HACERLO BIEN PORQUE ES UNA PUTA MIERDA

        if (angleInt < 0) angleInt = angleInt * -1;
        if (angleInt >= 360) angleInt = 360;
        if (angleInt > 5 && angleInt <= 29) angleInt = 20;
        if (angleInt >= 30 && angleInt <= 50) angleInt = 40;
        if (angleInt >= 0 && angleInt <= 5) angleInt = 360;
        if (angleInt >= 51 && angleInt <= 69) angleInt = 60;
        if (angleInt >= 70 && angleInt <= 139) angleInt = 135;
        if (angleInt >= 140 && angleInt <= 169) angleInt = 160; // ESTA LA HACE RARA
        if (angleInt >= 170 && angleInt <= 200) angleInt = 200;
        if (angleInt >= 201 && angleInt <= 260) angleInt = 250;
        if (angleInt >= 261 && angleInt <= 280) angleInt = 270;
        if (angleInt >= 281 && angleInt <= 350) angleInt = 340;
        if (angleInt >= 351 && angleInt <= 359) angleInt = 360;
        mainModule.startRotationY = new ParticleSystem.MinMaxCurve(angleInt);
        //Debug.Log(mainModule.startRotationY.constant);

        mainModule.startRotationY = new ParticleSystem.MinMaxCurve(angleInt);


        StartCoroutine(ReactivateObjects());
        slashController.finalDamage = enemyBaseDamage + powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
        slashController.pushForce = enemyBasePushForce;

    }
    IEnumerator ReactivateObjects()
    {
        yield return new WaitForSeconds(0.1f); // Ajusta el tiempo segÃºn sea necesario

        slashCollider.SetActive(true);
        slashParticle.SetActive(true);
    }
}
