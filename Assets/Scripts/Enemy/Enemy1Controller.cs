using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] private float HP;
    [SerializeField] private int damageValue;
    [SerializeField] private float inmuneTime;
    [SerializeField] private float normalAttackSpeed;
    [SerializeField] private float specialAttackSpeed;
    private Rigidbody rb;
    private float timer;
    private Animator animator;
    private bool onlyOnceSpecial,onlyOnceAttack;
    NavMeshAgent navMeshAgent;

    //SLASH STUFF
    public GameObject boundCharacter;
    public GameObject SlashEffect;
    private int veces;
    [SerializeField]
    private Transform slashDirection;

    [SerializeField]
    private GameObject slashParticle;

    [SerializeField]
    private GameObject slashCollider;

    private ParticleSystem slashParticleSystem;
    private Vector3 evadeAttackDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        evadeAttackDirection = -transform.forward;
        onlyOnceSpecial = true;
        onlyOnceAttack = true;
         slashParticleSystem = slashParticle.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }

    }
    public void TakeDamage(int damageAmount)
    {
        if (timer <= 0)
        {
            timer = inmuneTime;
            HP -= damageAmount;
            if (HP < 0)
            {
                animator.SetTrigger("die");
                animator.SetBool("isChasing",false);
            }
            else
            {
                //if(!animator.GetBool("isEvading")) 
                    animator.SetTrigger("damage");               
            }
        }



    }
    private void FixedUpdate()
    {
        if (animator.GetBool("attackOn"))
        {
            //mete slash normal (SI NO FUNCIONA AQUI, METELO EN LA FUNCION DE ABAJO SLASH
            //  Slash();
            rb.MovePosition(transform.position + transform.forward * normalAttackSpeed * Time.fixedDeltaTime);
            navMeshAgent.updatePosition = false;
            onlyOnceAttack = false;
        }
        if (!animator.GetBool("attackOn") && !onlyOnceAttack)
        {
            navMeshAgent.updatePosition = true;
            onlyOnceAttack = true;
        }
        if (animator.GetBool("isEvading"))
        {
            //navMeshAgent.enabled = false;
            //METE SLASH ESPECIAL
            rb.MovePosition(transform.position + evadeAttackDirection * normalAttackSpeed * Time.fixedDeltaTime);
            navMeshAgent.updatePosition = false;
            onlyOnceSpecial = false;
            //Debug.Log("entraaqui");
        }
        if (!animator.GetBool("isEvading") && !onlyOnceSpecial)
        {
            navMeshAgent.updatePosition = true;
            onlyOnceSpecial = true;
        }

    }

public void Slash()
{
        veces++;
        Debug.Log(veces);
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


    }
    IEnumerator ReactivateObjects()
    {
        yield return new WaitForSeconds(0.1f); // Ajusta el tiempo segÃºn sea necesario

        slashCollider.SetActive(true);
        slashParticle.SetActive(true);
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SlashEffect"))
        {
            TakeDamage(damageValue);
        }
    }

}
