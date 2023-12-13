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
    private Rigidbody rb;
    private Animator animator;
    private bool onlyOnceSpecial,onlyOnceAttack;
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
    private GameObject slashCollider;
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
        /*
        if (animator.GetBool("isEvading"))
        {
            //navMeshAgent.enabled = false;
            //METE SLASH ESPECIAL
            navMeshAgent.updatePosition = false;
            rb.MovePosition(transform.position + evadeAttackDirection * normalAttackSpeed * Time.fixedDeltaTime);
            onlyOnceSpecial = false;
            Debug.Log("entraaqui");
        }
        if (!animator.GetBool("isEvading") && !onlyOnceSpecial)
        {
            navMeshAgent.updatePosition = true;
            onlyOnceSpecial = true;
            Debug.Log("entraaqui");
        }
        */

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
      //  if (newAngle < 0) newAngle = newAngle * -1;
        if (newAngle >= 360) newAngle = 360;
        if (newAngle >= 0 && newAngle <= 5) newAngle = 360;

        mainModule.startRotationY = new ParticleSystem.MinMaxCurve(newAngle);

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
