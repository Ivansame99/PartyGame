using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] private float HP;
    [SerializeField] private int damageValue;
    [SerializeField] private float inmuneTime; 
    [SerializeField] private float specialAttackSpeed;
    private Rigidbody rb;
    private float timer;
    private Animator animator;
    private bool onlyOnce;
    NavMeshAgent navMeshAgent;

    private Vector3 evadeAttackDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        evadeAttackDirection = -transform.forward;
        onlyOnce = true;
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
        
        }
        if (animator.GetBool("isEvading"))
        {
            //navMeshAgent.enabled = false;
            //METE SLASH ESPECIAL
            rb.MovePosition(transform.position + evadeAttackDirection * specialAttackSpeed * Time.fixedDeltaTime);
            navMeshAgent.updatePosition = false;
            onlyOnce = false;
            //Debug.Log("entraaqui");
        }
        if (!animator.GetBool("isEvading") && !onlyOnce)
        {
            navMeshAgent.updatePosition = true;
            onlyOnce = true;
        }

    }

    public void Slash()
    {

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
